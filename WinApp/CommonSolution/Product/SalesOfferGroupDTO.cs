/********************************************************************************************
 * Project Name - Sales Offer Group DTO
 * Description  - Data object of sales offer group DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        28-May-2017   Raghuveera    Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the sales offer group data object class. This acts as data holder for the sales offer group business object
    /// </summary>
    public class SalesOfferGroupDTO
    {
        /// <summary>
        /// SearchBySalesOfferGroupParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchBySalesOfferGroupParameters
        {
            /// <summary>
            /// Search by SALE_GROUP_ID field
            /// </summary>
            SALE_GROUP_ID = 0,
            /// <summary>
            /// Search by NAME field
            /// </summary>
            NAME = 1,
            /// <summary>
            /// Search by IS_UPSELL field
            /// </summary>
            IS_UPSELL = 2,            
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE = 3,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID = 4,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 5
        }
        int saleGroupId;
        string name;
        bool isUpsell;
        bool isActive;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdatedDate;
        int siteId;
        string guid;
        bool synchStatus;
        int masterEntityId;
        List<SaleGroupProductMapDTO> saleGroupProductMapDTOList;

        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Contructor
        /// </summary>
        public SalesOfferGroupDTO()
        {
            log.Debug("Starts-SalesOfferGroupDTO() default constructor.");
            saleGroupId = -1;            
            masterEntityId = -1;
            isUpsell = false;
            isActive = true;
            siteId = -1;
            log.Debug("Ends-SalesOfferGroupDTO() default constructor.");
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>        
        public SalesOfferGroupDTO(int saleGroupId, string name, bool isUpsell, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                 DateTime lastUpdatedDate, int siteId, string guid, bool synchStatus, int masterEntityId)
        {
            log.Debug("Starts-SalesOfferGroupDTO(with all the data fields) Parameterized constructor.");
            this.saleGroupId = saleGroupId;
            this.name = name;
            this.isUpsell = isUpsell;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.Debug("Ends-SalesOfferGroupDTO(with all the data fields) Parameterized constructor.");
        }
        /// <summary>
        /// Get/Set method of the SaleGroupId field
        /// </summary>
        [DisplayName("SaleGroupId")]
        [ReadOnly(true)]
        public int SaleGroupId { get { return saleGroupId; } set { saleGroupId = value;} }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the IsUpsell field
        /// </summary>
        [DisplayName("IsUpsell")]
        public bool IsUpsell { get { return isUpsell; } set { isUpsell = value; this.IsChanged = true; } }

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
        /// Get/Set method of the SaleGroupProductMapDTOList field
        /// </summary>
        [DisplayName("SaleGroupProductMapDTOList")]
        public List<SaleGroupProductMapDTO> SaleGroupProductMapDTOList { get { return saleGroupProductMapDTOList; } set { saleGroupProductMapDTOList = value; } }

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
                    return notifyingObjectIsChanged || saleGroupId < 0;
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
