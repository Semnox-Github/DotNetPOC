/********************************************************************************************
 * Project Name - Sale Group Product Map DTO
 * Description  - Data object of sale group product map DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        28-May-2017   Raghuveera    Created 
 ********************************************************************************************/
using Semnox.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the sale group product map data object class. This acts as data holder for the sale group product map business object
    /// </summary>
    public class SaleGroupProductMapDTO
    {
        /// <summary>
        /// SearchBySaleGroupProductMapParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchBySaleGroupProductMapParameters
        {
            /// <summary>
            /// Search by TYPE_MAP_ID field
            /// </summary>
            TYPE_MAP_ID = 0,
            /// <summary>
            /// Search by SALE_GROUP_ID field
            /// </summary>
            SALE_GROUP_ID = 1,
            /// <summary>
            /// Search by PRODUCT_ID field
            /// </summary>
            PRODUCT_ID = 2,
            /// <summary>
            /// Search by SQUENCE_ID field
            /// </summary>
            SQUENCE_ID = 3,  
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE = 4,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID = 5,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 6
        }
        int typeMapId;
        int saleGroupId;
        int productId;
        int sequenceId;
        bool isActive;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdatedDate;
        int siteId;
        string guid;
        bool synchStatus;
        int masterEntityId;

        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Contructor
        /// </summary>
        public SaleGroupProductMapDTO()
        {
            log.Debug("Starts-SaleGroupProductMapDTO() default constructor.");
            typeMapId = -1;
            saleGroupId = -1;            
            masterEntityId = -1;
            productId = -1;
            sequenceId = -1;
            isActive = true;
            siteId = -1;
            log.Debug("Ends-SaleGroupProductMapDTO() default constructor.");
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>        
        public SaleGroupProductMapDTO(int typeMapId, int saleGroupId, int productId, int sequenceId, bool isActive, string createdBy, DateTime creationDate
                                , string lastUpdatedBy, DateTime lastUpdatedDate, int siteId, string guid, bool synchStatus, int masterEntityId)
        {
            log.Debug("Starts-SaleGroupProductMapDTO(with all the data fields) Parameterized constructor.");
            this.typeMapId = typeMapId;
            this.saleGroupId = saleGroupId;
            this.productId = productId;
            this.sequenceId = sequenceId;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.Debug("Ends-SaleGroupProductMapDTO(with all the data fields) Parameterized constructor.");
        }
        /// <summary>
        /// Get/Set method of the TypeMapId field
        /// </summary>
        [DisplayName("Map Id")]
        [ReadOnly(true)]
        public int TypeMapId { get { return typeMapId; } set { typeMapId = value; } }

        /// <summary>
        /// Get/Set method of the SaleGroupId field
        /// </summary>
        [DisplayName("Sale Group")]
        public int SaleGroupId { get { return saleGroupId; } set { saleGroupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("Product")]
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SquenceId field
        /// </summary>
        [DisplayName("Display Order")]
        public int SequenceId { get { return sequenceId; } set { sequenceId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUserId field
        /// </summary>
        [DisplayName("LastUpdatedUserId")]
        [Browsable(false)]
        public string LastUpdatedUserId { get { return lastUpdatedBy; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; }  }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        ///  Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || typeMapId < 0;
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
            log.Debug("Starts-AcceptChanges() method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() method.");
        }
    }
}
