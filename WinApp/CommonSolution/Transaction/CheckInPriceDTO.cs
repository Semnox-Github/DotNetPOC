/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of CheckInPrice
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        22-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the CheckInPrice data object class. This acts as data holder for the CheckInPrice business object
    /// </summary>
    public class CheckInPriceDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by    ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by    ID field
            /// </summary>
            ID_LIST,
            /// <summary>
            /// Search by PRODUCT ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by  TIME SLAB field
            /// </summary>
            TIME_SLAB,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int id;
        private int productId;
        private int timeSlab;
        private decimal price;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int masterEntityId;
        private DateTime creationDate;
        private string createdBy;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CheckInPriceDTO()
        {
            log.LogMethodEntry();
            id = -1;
            productId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public CheckInPriceDTO(int id, int productId, int timeSlab, decimal price)
            :this()
        {
            log.LogMethodEntry(id, productId, timeSlab, price);
            this.id = id;
            this.productId = productId;
            this.timeSlab = timeSlab;
            this.price = price;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public CheckInPriceDTO(int id,int productId,int timeSlab, decimal price,int siteId,string guid, bool synchStatus,
                                DateTime lastUpdatedDate,string lastUpdatedBy,int masterEntityId, DateTime creationDate,
                                string createdBy)
            :this(id, productId, timeSlab, price)
        {
            log.LogMethodEntry(id, productId, timeSlab, price, siteId, guid, synchStatus,lastUpdatedDate, lastUpdatedBy, 
                               masterEntityId, creationDate,createdBy);
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.masterEntityId = masterEntityId;
            this.siteId = siteId;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId
        {
            get { return productId; }
            set { productId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TimeSlab field
        /// </summary>
        public int TimeSlab
        {
            get { return timeSlab; }
            set { timeSlab = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>
        public decimal Price
        {
            get { return price; }
            set { price = value; this.IsChanged = true; }
        }
      

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value;  }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value;  }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value;  }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }
        ///// <summary>
        ///// Get/Set method of the ActiveFlag field
        ///// </summary>
        //public bool ActiveFlag
        //{
        //    get { return activeFlag; }
        //    set { activeFlag = value; this.IsChanged = true; }
        //}

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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}
