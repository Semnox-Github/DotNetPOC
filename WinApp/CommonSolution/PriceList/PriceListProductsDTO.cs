/********************************************************************************************
 * Project Name - PriceList
 * Description  - Data object of the PriceListProducts 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.60        05-Feb-2019    Indrajeet Kumar  Created
 *2.70.2        30-Jul-2019    Deeksha          Modifications as per three tier standard.
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.PriceList
{
    public class PriceListProductsDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Search By Parameters
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// PRICELISTPRODUCT ID search field
            /// </summary>
            PRICELISTPRODUCT_ID,
            /// <summary>
            /// PRICELIST ID search field
            /// </summary>
            PRICELIST_ID,
            /// <summary>
            /// PRODUCT ID search field
            /// </summary>
            PRODUCT_ID,
            // <summary>
            /// SITE ID search field
            /// </summary>
            SITE_ID,
            // <summary>
            ///  MASTERENTITY ID search field
            /// </summary>
            MASTERENTITY_ID,
            // <summary>
            ///  ISACTIVE search field
            /// </summary>
            ISACTIVE
        }

        private int priceListProductId;
        private int priceListId;
        private int productId;
        private decimal price;
        private DateTime? effectiveDate;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string guid;
        private bool synchStatus;
        private int site_id;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool isActive;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PriceListProductsDTO()
        {
            log.LogMethodEntry();
            this.priceListProductId = -1;
            this.priceListId = -1;
            this.productId = -1;
            this.site_id = -1;
            this.masterEntityId = -1;
            this.isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with required fields.
        /// </summary>
        public PriceListProductsDTO(int priceListProductId, int priceListId, int productId, decimal price, DateTime? effectiveDate, bool isActive)
            :this()
        {
            log.LogMethodEntry(priceListProductId, priceListId, productId, price, effectiveDate, isActive);
            this.priceListProductId = priceListProductId;
            this.priceListId = priceListId;
            this.productId = productId;
            this.price = price;
            this.effectiveDate = effectiveDate;           
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the fields.
        /// </summary>
        public PriceListProductsDTO(int priceListProductId, int priceListId, int productId, decimal price, DateTime? effectiveDate, DateTime lastUpdatedDate,
                                    string lastUpdatedBy, string guid, bool synchStatus, int site_id, int masterEntityId, string createdBy, DateTime creationDate, bool isActive)
            :this(priceListProductId, priceListId, productId, price, effectiveDate, isActive)
        {
            log.LogMethodEntry(priceListProductId, priceListId, productId, price, effectiveDate, lastUpdatedDate, lastUpdatedBy, guid, synchStatus, site_id, masterEntityId, createdBy, creationDate, isActive);

            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.site_id = site_id;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set for PriceListProductId
        /// </summary>
        public int PriceListProductId { get { return priceListProductId; } set { priceListProductId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for PriceListId
        /// </summary>
        public int PriceListId { get { return priceListId; } set { priceListId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for ProductId
        /// </summary>
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Price
        /// </summary>
        public decimal Price { get { return price; } set { price = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for EffectiveDate
        /// </summary>
        public DateTime? EffectiveDate { get { return effectiveDate; } set { effectiveDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for LastUpdatedDate
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value;  } }

        /// <summary>
        /// Get/Set for LastUpdatedBy
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set for Guid
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for SynchStatus
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set for Site_id
        /// </summary>
        public int Site_id { get { return site_id; } set { site_id = value;  } }

        /// <summary>
        /// Get/Set for MasterEntityId
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for CreatedBy
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }

        /// <summary>
        /// Get/Set for CreationDate
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value;  } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || priceListProductId < 0;
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
