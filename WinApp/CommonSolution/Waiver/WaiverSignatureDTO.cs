/********************************************************************************************
 * Project Name - Waiver
 * Description  - Data object of the WaiverSignature
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.70          01-Jul -2019   Girish Kundar    Modified : Added Parametrized Constructor with required fields.
 *2.70.2        27-Sep-2019    Deeksha          Waiver phase 2 -Modified constructors to Remove  decommissioned fields.
 *2.70.2        03-Jan-2020    Akshay G         Added searchParameter - LINE_ID_IN
 *2.140.0       14-Sep-2021    Guru S A         Waiver mapping UI enhancements
 ******************************************************************************************** */
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Semnox.Parafait.Waiver
{
    public enum WaiverSigningMode
    {
        NONE,
        ONLINE,
        DEVICE,
        MANUAL
    };

    /// <summary>
    ///  This is the user data object class. This acts as data holder for the user business object
    /// </summary> 
    public class WaiverSignatureDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();


        public enum WaiverSignatureChannel
        {
            /// <summary>
            /// None
            /// </summary>
            NONE,
            /// <summary>
            /// POS
            /// </summary>
            POS,
            /// <summary>
            /// Tablet
            /// </summary>
            TABLET,
            /// <summary>
            /// Website
            /// </summary>
            WEBSITE,
            /// <summary>
            /// Kiosk
            /// </summary>
            KIOSK 
        }
        /// <summary>
        /// SearchByWaiverParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByWaiverSignatureParameters
        {
            /// <summary>
            /// Search by WAIVER SIGNED ID field
            /// </summary>
            WAIVER_SIGNED_ID,
            /// <summary>
            /// Search by CUSTOMER SIGNED WAIVER ID field
            /// </summary>
            CUSTOMER_SIGNED_WAIVER_ID,
            /// <summary>
            /// Search by WAIVERSETDETAIL_ID field
            /// </summary>
            WAIVERSETDETAIL_ID,
            ///// <summary>
            ///// Search by WAIVER SIGNED FILENAME field
            ///// </summary>
            //WAIVER_SIGNED_FILENAME ,
            ///// <summary>
            ///// Search by EXPIRY DATE field
            ///// </summary>
            //EXPIRY_DATE ,
            /// <summary>
            /// Search by TRX ID field
            /// </summary>
            TRX_ID,
            /// <summary>
            /// Search by LINE ID field
            /// </summary>
            LINE_ID,
            /// <summary>
            /// Search by USER ID field
            /// </summary>
            USER_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by LAST UPDATED DATE field
            /// </summary>
            LAST_UPDATED_DATE,
            /// <summary>
            /// Search by LAST UPDATED BY field
            /// </summary>
            LAST_UPDATED_BY,
            /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by SYNCH_STATUS field
            /// </summary>
            SYNCH_STATUS,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by LINE_ID_IN field
            /// </summary>
            LINE_ID_IN
        }

        private int waiverSignedId;
        private int waiverSetDetailId; 
        private int trxId;
        private int lineId; 
        private int userId;
        private string signedMode; 
        private bool isActive;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string guid;
        private int site_id;
        private bool synchStatus;
        private int masterEntityId;
        private int productId;
        private string productName;
        private int customerSignedWaiverId;
        private int customerId;
        private string customerSignedWaiverFileName;
        private bool isOverriden;

        /// <summary>
        /// Default constructor
        /// </summary>
        public WaiverSignatureDTO()
        {
            log.LogMethodEntry();
            waiverSignedId = -1;
            customerSignedWaiverId = -1;
            customerId = -1;
            waiverSetDetailId = -1;
            trxId = -1;
            lineId = -1;
            // customerId = -1;
            masterEntityId = -1;
            productId = -1;
            userId = -1;
            synchStatus = false;
            isActive = true;
            site_id = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required  data fields
        /// </summary>
        public WaiverSignatureDTO(int waiverSignedId, int waiverSetDetailId, int trxId, int lineId, int userId, string signedMode, bool isActive, int customerSignedWaiverId, bool isOverriden)
        {
            log.LogMethodEntry(waiverSignedId, trxId, lineId, userId, signedMode, isActive, customerSignedWaiverId, isOverriden);
            this.waiverSignedId = waiverSignedId;
            this.waiverSetDetailId = waiverSetDetailId; 
            this.trxId = trxId;
            this.lineId = lineId; 
            this.signedMode = signedMode; 
            this.userId = userId;
            this.isActive = isActive; 
            this.customerSignedWaiverId = customerSignedWaiverId;
            this.isOverriden = isOverriden;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public WaiverSignatureDTO(int waiverSignedId, int waiverSetDetailId,
                                int trxId, int lineId, int userId, string signedMode, bool isActive,
                                DateTime creationDate, string createdBy, DateTime lastUpdatedDate, string lastUpdatedBy,
                                string guid, int site_id, bool synchStatus, int masterEntityId, int customerSignedWaiverId, string customerSignedWaiverFileName,
                                bool isOverriden)
            : this(waiverSignedId, waiverSetDetailId, trxId, lineId, userId, signedMode, isActive, customerSignedWaiverId, isOverriden)
        {
            log.LogMethodEntry(waiverSignedId, waiverSetDetailId, trxId, lineId, userId, signedMode, isActive, creationDate, createdBy,
                                lastUpdatedDate, lastUpdatedBy, guid, site_id, synchStatus, masterEntityId, customerSignedWaiverId, customerSignedWaiverFileName,
                                isOverriden);
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.site_id = site_id;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.customerSignedWaiverFileName = customerSignedWaiverFileName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the WaiverSignedId field
        /// </summary>
        [DisplayName("Waiver Signed Id")]
        [ReadOnly(true)]
        public int WaiverSignedId { get { return waiverSignedId; } set { waiverSignedId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the WaiverSetDetailId field
        /// </summary>
        [DisplayName("Waiver Set Detail Id ")]
        [ReadOnly(true)]
        public int WaiverSetDetailId { get { return waiverSetDetailId; } set { waiverSetDetailId = value; this.IsChanged = true; } }
         

        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        [DisplayName("TrxId")]
        [ReadOnly(true)]
        public int TrxId { get { return trxId; } set { trxId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the LineId field
        /// </summary>
        [DisplayName("LineId")]
        [ReadOnly(true)]
        public int LineId { get { return lineId; } set { lineId = value; this.IsChanged = true; } } 
        /// <summary>
        /// Get/Set method of the SignedMode field
        /// </summary>
        [DisplayName("Signed Mode")]
        public string SignedMode { get { return signedMode; } set { signedMode = value; this.IsChanged = true; } } 
        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        [DisplayName("UserId")]
        [ReadOnly(true)]
        public int UserId { get { return userId; } set { userId = value; this.IsChanged = true; } } 
        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active?")]
        [ReadOnly(true)]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } } 
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [ReadOnly(true)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        [ReadOnly(true)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } } 
        /// <summary>
        /// Get/Set method of the GUID field
        /// </summary>
        [DisplayName("GUID")]
        [ReadOnly(true)]
        public string GUID { get { return guid; } set { guid = value; this.IsChanged = true; } } 
        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site_id")]
        [ReadOnly(true)]
        public int Site_id { get { return site_id; } set { site_id = value; this.IsChanged = true; } } 
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [ReadOnly(true)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } } 
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [ReadOnly(true)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } } 
        [NotMapped]
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("ProductId")]
        public int ProductId { get { return productId; } set { productId = value; } }

        [NotMapped]
        /// <summary>
        /// Get/Set method of the ProductName field
        /// </summary>
        [DisplayName("ProductName")]
        public string ProductName { get { return productName; } set { productName = value; } } 
        /// <summary>
        /// Get/Set method of the WaiverFile field
        /// </summary>
        public byte[] WaiverFile { get; set; }
        /// <summary>
        /// Get/Set method of the GUID field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get/Set method of the CustomerSignedWaiverId  field
        /// </summary>
        [DisplayName("CustomerSignedWaiverId ")]
        [ReadOnly(true)]
        public int CustomerSignedWaiverId { get { return customerSignedWaiverId; } set { customerSignedWaiverId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CustomerSignedWaiverFileName field
        /// </summary>
        [DisplayName("Customer Signed Waiver File Name")]
        [ReadOnly(true)]
        public string CustomerSignedWaiverFileName { get { return customerSignedWaiverFileName; } set { customerSignedWaiverFileName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        /// <summary>
        /// Get/Set method of the isOverriden field
        /// </summary>
        [DisplayName("Overriden?")] 
        public bool IsOverriden { get { return isOverriden; } set { isOverriden = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        public int CustomerId { get { return customerId; } set { customerId = value; } }
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || waiverSignedId < 0;
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
