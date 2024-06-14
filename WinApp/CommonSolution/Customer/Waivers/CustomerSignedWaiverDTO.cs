/********************************************************************************************
 * Project Name - CustomerSignedWaiver DTO
 * Description  - Data object of CustomerSignedWaiver
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70.2       26-Sep-2019      Deeksha        Created for waiver phase 2
 *2.100        19-Oct-2020      Guru S A       Enabling minor signature option for waiver
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace Semnox.Parafait.Customer.Waivers
{
    public class CustomerSignedWaiverDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCSWParameters
        {
            /// <summary>
            /// Search by CUSTOMER SIGNED WAIVER ID field
            /// </summary>
            CUSTOMER_SIGNED_WAIVER_ID,

            /// <summary>
            /// Search by CUSTOMER SIGNED WAIVER HEADER ID field
            /// </summary>
            CUSTOMER_SIGNED_WAIVER_HEADER_ID,

            /// <summary>
            /// Search by SIGNED WAIVER FILE NAME LIST field
            /// </summary>
            SIGNED_WAIVER_FILE_NAME,

            /// <summary>
            /// Search by CUSTOMER WAIVER SET DETAIL ID field
            /// </summary>
            WAIVER_SET_DETAIL_ID,

            /// <summary>
            /// Search by EXPIRY DATE field
            /// </summary>
            EXPIRY_DATE,

            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,

            /// <summary>
            /// Search by CUSTOMER SIGNED WAIVER HEADER ID LIST field
            /// </summary>
            CUSTOMER_SIGNED_WAIVER_HEADER_ID_LIST,
            /// <summary>
            /// Search by SIGNED FOR field
            /// </summary>
            SIGNED_FOR,
            /// <summary>
            /// Search by SIGNED By field
            /// </summary>
            SIGNED_BY,
            /// <summary>
            /// Search by IGNORE EXPIRED field
            /// </summary> 
            IGNORE_EXPIRED,
            /// <summary>
            /// Search by WAIVER_CODE field
            /// </summary> 
            WAIVER_CODE,
            /// <summary>
            /// Search by WAIVER SET ID field
            /// </summary>
            WAIVER_SET_ID,
            /// <summary>
            /// Search by FACILITY_ID field
            /// </summary>
            FACILITY_ID,
            /// <summary>
            /// Search by CUSTOMER_SIGNED_WAIVER_ID_LIST field
            /// </summary>
            CUSTOMER_SIGNED_WAIVER_ID_LIST,
            /// <summary>
            /// Search by PRODUCT_ID_LIST field
            /// </summary>
            PRODUCT_ID_LIST,
            /// <summary>
            /// Search by LAST_UPDATE_FROM_DATE field
            /// </summary>
            LAST_UPDATE_FROM_DATE,
            /// <summary>
            /// Search by LAST_UPDATE_TO_DATE field
            /// </summary>
            LAST_UPDATE_TO_DATE,
            /// <summary>
            /// Search by SIGNED_FOR_IN field
            /// </summary>
            SIGNED_FOR_IN
        }            
        private int customerSignedWaiverId;
        private int customerSignedWaiverHeaderId;
        private int waiverSetDetailId;
        private string waiverName;
        private string waiverFileName;
        private string signedWaiverFileName;
        private int signedFor;
        private string signedForName;
        private DateTime? expiryDate;
        private bool isActive;
        private int? deactivatedBy;
        private DateTime? deactivationDate;
        private int? deactivationApprovedBy;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        private int signedBy;
        private string signedByName;
        private DateTime signedDate;
        private string waiverCode;
        private int waiverSetId;
        private string waiverSetDescription; 
        private List<WaiveSignatureImageWithCustomerDetailsDTO> waiverSignedImageList;
        private List<CustomerContentForWaiverDTO> customerContentList;
        private string signedWaiverFileContentInBase64Format;
        private int guardianId;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CustomerSignedWaiverDTO()
        {
            log.LogMethodEntry();
            customerSignedWaiverId = -1;
            customerSignedWaiverHeaderId = -1;
            waiverSetDetailId = -1;
            signedFor = -1;
            deactivatedBy = -1;
            deactivationApprovedBy = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            customerContentList = new List<CustomerContentForWaiverDTO>();
            signedBy = -1;
            waiverSetId = -1;
            waiverSignedImageList = new List<WaiveSignatureImageWithCustomerDetailsDTO>();
            guardianId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public CustomerSignedWaiverDTO(int customerSignedWaiverId, int customerSignedWaiverHeaderId, int waiverSetDetailId, string signedWaiverFileName,
                                     int signedFor, DateTime? expiryDate, bool isActive, int? deactivatedBy, DateTime? deactivationDate,
                                     int? deactivationApprovedBy, string waiverFileName)
                    : this()
        {
            log.LogMethodEntry(customerSignedWaiverId, customerSignedWaiverHeaderId, waiverSetDetailId, signedWaiverFileName,
                                     signedFor, expiryDate, isActive, deactivatedBy, deactivationDate, deactivationApprovedBy, waiverFileName);
            this.customerSignedWaiverId = customerSignedWaiverId;
            this.customerSignedWaiverHeaderId = customerSignedWaiverHeaderId;
            this.waiverSetDetailId = waiverSetDetailId;
            this.signedWaiverFileName = signedWaiverFileName;
            this.signedFor = signedFor;
            this.expiryDate = expiryDate;
            this.isActive = isActive;
            this.deactivatedBy = deactivatedBy;
            this.deactivationDate = deactivationDate;
            this.deactivationApprovedBy = deactivationApprovedBy;
            this.waiverFileName = waiverFileName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the  fields
        /// </summary>
        public CustomerSignedWaiverDTO(int customerSignedWaiverId, int customerSignedWaiverHeaderId, int waiverSetDetailId, string signedWaiverFileName,
                                     int signedFor, string signedForName, DateTime? expiryDate, bool isActive, int? deactivatedBy, DateTime? deactivationDate,
                                     int? deactivationApprovedBy, string guid, int siteId, bool synchStatus, int masterEntityId, string createdBy,
                                    DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, int signedBy, string signedByName, DateTime signedDate, string waiverCode, 
                                     int waiverSetId, string waiverSetDescription, string waiverName, string waiverFileName)
                    : this(customerSignedWaiverId, customerSignedWaiverHeaderId, waiverSetDetailId, signedWaiverFileName,
                                     signedFor, expiryDate, isActive, deactivatedBy, deactivationDate, deactivationApprovedBy, waiverFileName)
        {
            log.LogMethodEntry(customerSignedWaiverId, customerSignedWaiverHeaderId, waiverSetDetailId, waiverName, waiverFileName, signedWaiverFileName,
                                      signedFor, signedForName, expiryDate, isActive, deactivatedBy, deactivationDate, deactivationApprovedBy, guid, siteId, synchStatus, masterEntityId, createdBy,
                                     creationDate, lastUpdatedBy, lastUpdateDate, signedBy, signedByName, signedDate, waiverCode, waiverSetId, waiverSetDescription);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.signedForName = signedForName;
            this.signedBy = signedBy;
            this.signedByName = signedByName;
            this.signedDate = signedDate;
            this.waiverCode = waiverCode;
            this.waiverSetId = waiverSetId;
            this.waiverSetDescription = waiverSetDescription;
            this.waiverName = waiverName; 
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CustomerSignedWaiverId field
        /// </summary>
        [DisplayName("CustomerSignedWaiverId")]
        public int CustomerSignedWaiverId { get { return customerSignedWaiverId; } set { customerSignedWaiverId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustomerSignedWaiverId field
        /// </summary>
        [DisplayName("CustomerSignedWaiverHeaderId")]
        public int CustomerSignedWaiverHeaderId { get { return customerSignedWaiverHeaderId; } set { customerSignedWaiverHeaderId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the WaiverSetDetailId field
        /// </summary>
        [DisplayName("WaiverSetDetailId")]
        public int WaiverSetDetailId { get { return waiverSetDetailId; } set { waiverSetDetailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SignedWaiverFileName field
        /// </summary>
        [DisplayName("SignedWaiverFileName")]
        public string SignedWaiverFileName { get { return signedWaiverFileName; } set { signedWaiverFileName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the WaiverName field
        /// </summary>
        [DisplayName("WaiverName")]
        public string WaiverName { get { return waiverName; } set { waiverName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the waiverFileName field
        /// </summary>
        [DisplayName("WaiverFileName")]
        public string WaiverFileName { get { return waiverFileName; } set { waiverFileName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SignedFor field
        /// </summary>
        [DisplayName("SignedFor")]
        public int SignedFor { get { return signedFor; } set { signedFor = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SignedForName field
        /// </summary>
        [DisplayName("SignedForName")]
        public string SignedForName { get { return signedForName; } set { signedForName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        [DisplayName("ExpiryDate")]
        public DateTime? ExpiryDate { get { return expiryDate; } set { expiryDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DeactivatedBy field
        /// </summary>
        [DisplayName("DeactivatedBy")]
        public int? DeactivatedBy { get { return deactivatedBy; } set { deactivatedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DeactivationDate field
        /// </summary>
        [DisplayName("DeactivationDate")]
        public DateTime? DeactivationDate { get { return deactivationDate; } set { deactivationDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DeactivationApprovedBy field
        /// </summary>
        [DisplayName("DeactivationApprovedBy")]
        public int? DeactivationApprovedBy { get { return deactivationApprovedBy; } set { deactivationApprovedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; ; } }

        /// <summary>
        /// Get/Set method of the ValueInTickets field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the BarCode field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastModifiedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the lastModifiedDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the waiverSignedImageList field
        /// </summary>
        [DisplayName("WaiverSignedImageList")]
        [Browsable(false)]
        public List<WaiveSignatureImageWithCustomerDetailsDTO> WaiverSignedImageList { get { return  waiverSignedImageList; } set { waiverSignedImageList = value; } }

        /// <summary>
        /// Get/Set method of the PosProductExclusionDtoList field
        /// </summary>
        [Browsable(false)]
        public List<CustomerContentForWaiverDTO> CustomerContentForWaiverDTOList
        {
            get { return customerContentList; }
            set { customerContentList = value; }
        }

        /// <summary>
        /// Get/Set method of the SignedBy field
        /// </summary>
        [DisplayName("SignedBy")]
        public int SignedBy { get { return signedBy; } set { signedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SignedByName field
        /// </summary>
        [DisplayName("SignedByName")]
        public string SignedByName { get { return signedByName; } set { signedByName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SignedDate field
        /// </summary>
        [DisplayName("SignedDate")]
        public DateTime SignedDate { get { return signedDate; } set { signedDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the WaiverCode field
        /// </summary>
        [DisplayName("WaiverCode")]
        public string WaiverCode { get { return waiverCode; } set { waiverCode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the WaiverSetId field
        /// </summary>
        [DisplayName("WaiverSetId")]
        public int WaiverSetId { get { return waiverSetId; } set { waiverSetId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the WaiverSetDescription field
        /// </summary>
        [DisplayName("WaiverSetDescription")]
        public string WaiverSetDescription { get { return waiverSetDescription; } set { waiverSetDescription = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SignedWaiverFileContentInBase64Format field
        /// </summary>
        [DisplayName("Signed Waiver File Content In Base 64 Format")]
        [Browsable(false)]
        public string SignedWaiverFileContentInBase64Format { get { return signedWaiverFileContentInBase64Format; } set { signedWaiverFileContentInBase64Format = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GuardianId field
        /// </summary>
        [DisplayName("GuardianId")]
        public int GuardianId { get { return guardianId; } set { guardianId = value; this.IsChanged = true; } }


        /// <summary>
        ///Returns whether the CustomerContentForWaiverDTO changed or any of its CustomerContentForWaiverDTO children are changed
        /// </summary>
        [Browsable(false)]
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (customerContentList != null &&
                    customerContentList.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || customerSignedWaiverId < 0;
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