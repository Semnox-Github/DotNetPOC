/********************************************************************************************
 * Project Name - CustomerSignedWaiverHeader DTO
 * Description  - Data object of CustomerSignedWaiverHeader
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       26-Sep-2019    Deeksha        Created for waiver phase 2
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Customer.Waivers
{
    public class CustomerSignedWaiverHeaderDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCSWHeaderParameters
        {

            /// <summary>
            /// Search by CUSTOMER SIGNED WAIVER HEADER ID field
            /// </summary>
            CUSTOMER_SIGNED_WAIVER_HEADER_ID,

            /// <summary>
            /// Search by POS MACHINE ID field
            /// </summary>
            POS_MACHINE_ID,

            /// <summary>
            /// Search by SIGNED BY field
            /// </summary>
            SIGNED_BY,

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
        }

        private int customerSignedWaiverHeaderId;
        private int signedBy;
        private DateTime signedDate;
        private string channel;
        private int posMachineId;
        private bool isActive;
        private string waiverCode;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList;


        /// <summary>
        /// Default Constructor
        /// </summary>
        public CustomerSignedWaiverHeaderDTO()
        {
            log.LogMethodEntry();
            customerSignedWaiverHeaderId = -1;
            signedBy = -1;
            posMachineId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            customerSignedWaiverDTOList = new List<CustomerSignedWaiverDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields.
        /// </summary>
        public CustomerSignedWaiverHeaderDTO(int customerSignedWaiverHeaderId, int signedBy, DateTime signedDate, string channel,
                                              int posMachineId, bool isActive, string waiverCode)
            : this()
        {
            log.LogMethodEntry(customerSignedWaiverHeaderId, signedBy, signedDate, channel, posMachineId, isActive, waiverCode);
            this.customerSignedWaiverHeaderId = customerSignedWaiverHeaderId;
            this.signedBy = signedBy;
            this.signedDate = signedDate;
            this.channel = channel;
            this.posMachineId = posMachineId;
            this.isActive = isActive;
            this.waiverCode = waiverCode;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with All the fields.
        /// </summary>
        public CustomerSignedWaiverHeaderDTO(int customerSignedWaiverHeaderId, int signedBy, DateTime signedDate, string channel,
                                              int posMachineId, bool isActive, string waiverCode, string guid, int siteId,
                                              bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate,
                                              string lastUpdatedBy, DateTime lastUpdateDate)
            : this(customerSignedWaiverHeaderId, signedBy, signedDate, channel, posMachineId, isActive, waiverCode)
        {
            log.LogMethodEntry(customerSignedWaiverHeaderId, signedBy, signedDate, channel, posMachineId, isActive, waiverCode, guid, siteId,
                              synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
        }


        /// <summary>
        /// Get/Set method of the CustomerSignedWaiverHeaderId field
        /// </summary>
        [DisplayName("CustomerSignedWaiverHeaderId")]
        public int CustomerSignedWaiverHeaderId { get { return customerSignedWaiverHeaderId; } set { customerSignedWaiverHeaderId = value; this.IsChanged = true; } }

        // <summary>
        /// Get/Set method of the SignedBy field
        /// </summary>
        [DisplayName("SignedBy")]
        public int SignedBy { get { return signedBy; } set { signedBy = value; this.IsChanged = true; } }

        // <summary>
        /// Get/Set method of the SignedDate field
        /// </summary>
        [DisplayName("SignedDate")]
        public DateTime SignedDate { get { return signedDate; } set { signedDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Channel field
        /// </summary>
        [DisplayName("Channel")]
        public string Channel { get { return channel; } set { channel = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PosMachineId field
        /// </summary>
        [DisplayName("PosMachineId")]
        public int PosMachineId { get { return posMachineId; } set { posMachineId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the WaiverCode field
        /// </summary>
        [DisplayName("WaiverCode")]
        public string WaiverCode { get { return waiverCode; } set { waiverCode = value; this.IsChanged = true; } }

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
        /// Get/Set method of the CustomerSignedWaiverDTO field
        /// </summary>
        [Browsable(false)]
        public List<CustomerSignedWaiverDTO> CustomerSignedWaiverDTOList
        {
            get { return customerSignedWaiverDTOList; }
            set { customerSignedWaiverDTOList = value; }
        }

        /// <summary>
        ///Returns whether the CustomerSignedWaiverDTO changed or any of its CustomerSignedWaiverDTO children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (customerSignedWaiverDTOList != null &&
                    customerSignedWaiverDTOList.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || customerSignedWaiverHeaderId < 0;
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

    /// <summary>
    /// SignedFileInformationDTO
    /// </summary>
    internal class SignedFileInformationDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int customerSignedWaiverHeaderId;
        private int waiverSetId;
        private int waiverId;
        private int signedBy;
        private int signedFor;
        private string signedWaiverFileName;
        internal SignedFileInformationDTO(int customerSignedWaiverHeaderId, int waiverSetId, int waiverId, int signedBy, int signedFor, string signedWaiverFileName)
        {
            log.LogMethodEntry(customerSignedWaiverHeaderId, waiverSetId, waiverId, signedBy, signedFor, signedWaiverFileName);
            this.customerSignedWaiverHeaderId = customerSignedWaiverHeaderId;
            this.waiverSetId = waiverSetId;
            this.waiverId = waiverId;
            this.signedBy = signedBy;
            this.signedFor = signedFor;
            this.signedWaiverFileName = signedWaiverFileName;
            log.LogMethodExit();
        }
        internal int CustomerSignedWaiverHeaderId { get { return customerSignedWaiverHeaderId; } }
        internal int WaiverId { get { return waiverId; } }
        internal int WaiverSetId { get { return waiverSetId; } }
        internal int SignedBy { get { return signedBy; } }
        internal int SignedFor { get { return signedFor; } }
        internal string SignedWaiverFileName { get { return signedWaiverFileName; } }
    }
}
