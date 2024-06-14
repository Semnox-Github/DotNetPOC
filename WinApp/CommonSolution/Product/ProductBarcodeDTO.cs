/********************************************************************************************
 * Project barCode - Product Barcode DTO
 * Description  - Data object of Product Barcode
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *1.00        30-Aug-2017   Indhu           Created 
 ********************************************************************************************
 *2.60        10-Apr-2019   Archana         Include/Exclude for redeemable products changes
 *2.70        13-June-2019  Nagesh Badiger  Changed isActive datatype string to Bool
 *2.110.00    30-Nov-2020   Abhishek        Modified : Modified to 3 Tier Standard 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the Product Barcode data object class.This acts as data holder for the Product Barcode business object
    /// </summary>
    public class ProductBarcodeDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID = 0,
            /// <summary>
            /// Search by PRODUCT ID field
            /// </summary>
            PRODUCT_ID = 1,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE = 2,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 3,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID = 4,
            /// <summary>
            /// Search by BARCODE field
            /// </summary>
            BARCODE = 5
        }

        private int id;
        private string barCode;
        private int productId;
        private bool isActive;
        private string lastUpdateduserid;
        private DateTime lastUpdatedDate;
        private int site_Id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Contructor
        /// </summary>
        public ProductBarcodeDTO()
        {
            log.LogMethodEntry();
            id = -1;
            masterEntityId = -1;
            isActive = true;
            site_Id = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameters
        /// </summary>
        public ProductBarcodeDTO(int id, string barCode, int productId, bool isActive)
            : this()
        {
            log.LogMethodEntry(id, barCode, productId, isActive);
            this.id = id;
            this.barCode = barCode;
            this.productId = productId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>        
        public ProductBarcodeDTO(int id, string barCode, int productId, bool isActive,
                                 DateTime lastUpdatedDate, string lastUpdateduserid, string createdBy,
                                 DateTime creationDate, int site_Id, string guid, bool synchStatus, int masterEntityId)
            : this(id, barCode, productId, isActive)
        {
            log.LogMethodEntry(id, barCode, productId, isActive, lastUpdatedDate, LastUpdatedUserId, createdBy,
                               creationDate, site_Id, guid, synchStatus, masterEntityId);
            this.lastUpdateduserid = lastUpdateduserid;
            this.lastUpdatedDate = lastUpdatedDate;
            this.site_Id = site_Id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id { get { return id; } set { IsChanged = true; id = value; } }

        /// <summary>
        /// Get/Set method of the BarCode field
        /// </summary>
        public string BarCode { get { return barCode; } set { IsChanged = true; barCode = value; } }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Product_Id { get { return productId; } set { IsChanged = true; productId = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { IsChanged = true; isActive = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUserId field
        /// </summary>
        public string LastUpdatedUserId { get { return lastUpdateduserid; } set { lastUpdateduserid = value; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return site_Id; } set { site_Id = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { IsChanged = true; masterEntityId = value; } }

        /// <summary>
        ///  Get/Set method to track changes to the object
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
            this.IsChanged = false;
            log.LogMethodExit();
        }

    }
}
