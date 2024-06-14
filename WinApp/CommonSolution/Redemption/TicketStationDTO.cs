/********************************************************************************************
 * Project Name - TicketReceipt
 * Description  - TicketStationDTO class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       11-July-2018      Archana           Added for Redemption Kiosk 
 *2.70.2.0      16-Sept -2019     Girish Kundar     Part of Ticket Eater enhancements. 
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Product Type List
    /// </summary>
    public static class TicketStationAlgorithm
    {        
        public const string DEFAULT = "NONE";
        public const string MODULO_TEN_WEIGHT_THREE = "MODULO_TEN_WEIGHT_THREE";
    }
    
    /// <summary>
    /// This is the ticket Station data object class. This acts as data holder for the ticket station business object
    /// </summary>
    public class TicketStationDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByTicketReceiptParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByTicketStationParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by TICKET_STATION_ID field
            /// </summary>
            TICKET_STATION_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by IS TICKET STATION TYPE field
            /// </summary>
            TICKET_STATION_TYPE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
        }

        /// <summary>
        /// TICKETSTATIONTYPE Enum
        /// </summary>
        public enum TICKETSTATIONTYPE
        {
            /// <summary>
            /// POS acting as ticket station
            /// </summary>
            POS_TICKET_STATION ,

            /// <summary>
            /// Physical ticket station
            /// </summary>
            PHYSICAL_TICKET_STATION 
        }
        private int id;
        private string ticketStationId;
        private int voucherLength;
        private bool checkDigit;
        private TICKETSTATIONTYPE ticketStationType;
        private int ticketLength;
        private string checkBitAlgorithm;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default constructor for TicketStationDTO class
        /// </summary>
        public TicketStationDTO()
        {
            log.LogMethodEntry();
            id = -1;
            voucherLength = 0;
            checkDigit = false;
            ticketStationType = TICKETSTATIONTYPE.PHYSICAL_TICKET_STATION;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with Required the data fields
        /// </summary>
        public TicketStationDTO(int id, string ticketStationId, int voucherLength, bool checkDigit, int ticketLength,
                TICKETSTATIONTYPE ticketStationType = TICKETSTATIONTYPE.PHYSICAL_TICKET_STATION, bool isActive = true, string checkBitAlgorithm ="Default/NA")
            : this()
        {
            log.LogMethodEntry(id, ticketStationId, voucherLength, checkDigit, ticketStationType, ticketLength, isActive);
            this.id = id;
            this.ticketStationId = ticketStationId;
            this.voucherLength = voucherLength;
            this.checkDigit = checkDigit;
            this.ticketStationType = ticketStationType;
            this.ticketLength = ticketLength;
            this.isActive = isActive;
            this.checkBitAlgorithm = checkBitAlgorithm;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TicketStationDTO(int id, string ticketStationId, int voucherLength, bool checkDigit, TICKETSTATIONTYPE ticketStationType, int ticketLength, bool isActive,
                        string checkBitAlgorithm , string guid, string createdBy ,DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,  int siteId, bool synchStatus, int masterEntityId )
            : this(id, ticketStationId, voucherLength, checkDigit, ticketLength ,ticketStationType, isActive , checkBitAlgorithm)
        {
            log.LogMethodEntry(id, ticketStationId, voucherLength, checkDigit, ticketStationType, ticketLength,  isActive,
                        checkBitAlgorithm, createdBy, creationDate, lastUpdatedDate, lastUpdatedBy, guid,  siteId,  synchStatus,  masterEntityId);
            this.creationDate = creationDate;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            log.LogMethodExit();
        }

        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get method of the TicketStationId field
        /// </summary>
        public string TicketStationId { get { return ticketStationId; } set { ticketStationId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CheckDigit field
        /// </summary>
        public bool CheckDigit { get { return checkDigit; } set { checkDigit = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the VoucherLength field
        /// </summary>
        public int VoucherLength { get { return voucherLength; } set { voucherLength = value; this.IsChanged = true; } }


        /// <summary>
        /// Get method of the TicketLength field
        /// </summary>
        public int TicketLength { get { return ticketLength; } set { ticketLength = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the checkBitAlgorithm field
        /// </summary>
        public string CheckBitAlgorithm { get { return checkBitAlgorithm; } set { checkBitAlgorithm = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the ticketStationType field
        /// </summary>
        public TICKETSTATIONTYPE TicketStationType { get { return ticketStationType; } set { ticketStationType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>        
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the Updated field
        /// </summary>        
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdated By field
        /// </summary>        
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>        
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
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
