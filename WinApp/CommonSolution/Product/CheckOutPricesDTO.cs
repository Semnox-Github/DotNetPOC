/********************************************************************************************
 * Project Name - Products  
 * Description  - Data object of CheckOutPricesDTO 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By                 Remarks          
 *********************************************************************************************
 *2.60        08-Feb-2019   Indrajeet Kumar             Created
 *2.100.0     14-Aug-2020   Girish Kundar               Modified : 3 Tier changes as part of phase -3 Rest API changes
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class CheckOutPricesDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// ID search field
            /// </summary>
            ID,
            /// <summary>
            /// PRODUCT_ID search field
            /// </summary>
            PRODUCT_ID,
            // <summary>
            /// SITE_ID search field
            /// </summary>
            SITE_ID,
            // <summary>
            ///  MASTERENTITY_ID search field
            /// </summary>
            MASTERENTITY_ID,
            // <summary>
            ///  ISACTIVE search field
            /// </summary>
            ISACTIVE

        }

      private int id;
      private int productId;
      private int timeSlab;
      private decimal price;
      private int site_id;
      private string guid;
      private bool synchStatus;
      private DateTime lastUpdateDate;
      private string lastUpdatedBy;                                                                                              
      private int masterEntityId;
      private string createdBy;
      private DateTime creationDate;
      private bool isActive;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CheckOutPricesDTO()
        {
            log.LogMethodEntry();
            this.id = -1;
            this.productId = -1;
            this.masterEntityId = -1;
            this.isActive = true;
            log.LogMethodExit();
        }


        public CheckOutPricesDTO(int id, int productId, int timeSlab, decimal price ,bool isActive)
            :this()
        {
            log.LogMethodEntry(id, productId, timeSlab, price, isActive);
            this.id = id;
            this.productId = productId;
            this.timeSlab = timeSlab;
            this.price = price;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  the data fields
        /// </summary>
        public CheckOutPricesDTO(int id, int productId, int timeSlab, decimal price, int site_id, string guid, bool synchStatus,
                                DateTime lastUpdateDate, string lastUpdatedBy, int masterEntityId, string createdBy,
                                DateTime creationDate, bool isActive)
            :this(id,productId, timeSlab, price,isActive)
        {
            log.LogMethodEntry(id, productId, timeSlab, price, site_id, guid, synchStatus, lastUpdateDate,
                               lastUpdatedBy, masterEntityId, createdBy, creationDate, isActive);
            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TimeSlab field
        /// </summary>
        public int TimeSlab { get { return timeSlab; } set { timeSlab = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>
        public decimal Price { get { return price; } set { price = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return site_id; } set { site_id = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

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
