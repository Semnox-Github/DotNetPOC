/********************************************************************************************
 * Project Name - Special Pricing Options DTO
 * Description  - Data object of Special Pricing Options Object
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By                     Remarks          
 *********************************************************************************************
 *2.60        13-Feb-2019   Indrajeet Kumar                 Created 
 *2.60        22-Mar-2019   Nagesh Badiger                  Removed index enumerations and datatype changed for isActive
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the Special Pricing Options data object class. This acts as data holder for the Special Pricing Options business object
    /// </summary>
    public class SpecialPricingOptionsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        /// <summary>
        /// SearchBySpecialPricingOptionsParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchBySpecialPricingOptionsParameters
        {
            /// <summary>
            /// Search by PRICING_ID field
            /// </summary>
            PRICING_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG,
            // <summary>
            ///  MASTERENTITY_ID search field
            /// </summary>
            MASTERENTITY_ID,
            /// <summary>
            /// PERCENTAGE search field
            /// </summary>
            PERCENTAGE
        }

        int pricingId;
        string pricingName;
        decimal percentage;
        bool activeFlag;
        string requiresManagerApproval;
        string guid;
        int site_id;
        bool synchStatus;
        int masterEntityId;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdateDate;

        List<ProductSpecialPricesDTO> productSpecialPricesList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SpecialPricingOptionsDTO()
        {
            log.Debug("Starts - SpecialPricingOptionsDTO() default Constructor.");
            this.pricingId = -1;
            this.masterEntityId = -1;
            this.activeFlag = true;
            log.Debug("Starts - SpecialPricingOptionsDTO() default Constructor.");
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public SpecialPricingOptionsDTO(int pricingId, string pricingName, decimal percentage, bool activeFlag, string requiresManagerApproval,
                                        string guid, int site_id, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate,
                                        string lastUpdatedBy, DateTime lastUpdateDate)
        {
            log.LogMethodEntry(pricingId, pricingName, percentage, activeFlag, requiresManagerApproval, guid, site_id, synchStatus,
                masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);

            this.pricingId = pricingId;
            this.pricingName = pricingName;
            this.percentage = percentage;
            this.activeFlag = activeFlag;
            this.requiresManagerApproval = requiresManagerApproval;
            this.guid = guid;
            this.site_id = site_id;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            productSpecialPricesList = new List<ProductSpecialPricesDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the PricingId field
        /// </summary>
        public int PricingId { get { return pricingId; } set { pricingId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PricingName field
        /// </summary>
        public string PricingName { get { return pricingName; } set { pricingName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Percentage field
        /// </summary>
        public decimal Percentage { get { return percentage; } set { percentage = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool ActiveFlag { get { return activeFlag; } set { activeFlag = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RequiresManagerApproval field
        /// </summary>
        public string RequiresManagerApproval { get { return requiresManagerApproval; } set { requiresManagerApproval = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return site_id; } set { site_id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for CreditPlusConsumptionRulesList Field
        /// </summary>
        public List<ProductSpecialPricesDTO> ProductSpecialPricesList { get { return productSpecialPricesList; } set { productSpecialPricesList = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || pricingId < 0;
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
