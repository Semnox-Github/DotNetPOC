/********************************************************************************************
 * Project Name - TokenCardInventory
 * Description  - Data object of the TokenCardInventoryDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00       6-July-2017   Amaresh          Created 
 *2.50.0     14-Dec-2018   Guru S A         Application security changes
 *2.60       22-Feb-2019   Nagesh Badiger   Added enum MachinesTypeEnums for WebManagement
 *2.70.2       23-Jul-2019   Girish Kundar    Modified : Added Constructor with required Parameter
 ********************************************************************************************/
using System; 
using System.ComponentModel; 

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// class of TokenCardInventoryDTO
    /// </summary>
    public class TokenCardInventoryDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByTokenCardInventoryParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByTokenCardInventoryParameters
        {
            /// <summary>
            /// Search by CARD_INVENTORY_KEY field
            /// </summary>
            CARD_INVENTORY_KEY ,

            /// <summary>
            /// Search by FROM_DATE field
            /// </summary>
            FROM_DATE,

            /// <summary>
            /// Search by ACTION field
            /// </summary>
            ACTION ,

            /// <summary>
            /// Search by TAG_TYPE field
            /// </summary>
            TAG_TYPE,

            /// <summary>
            /// Search by MACHINE_TYPE field
            /// </summary>
            MACHINE_TYPE ,

            /// <summary>
            /// Search by ACTIVITY_TYPE field
            /// </summary>
            ACTIVITY_TYPE,

            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID ,

            /// <summary>
            /// Search by TO_DATE field
            /// </summary>
            TO_DATE ,

            /// <summary>
            /// Search by DATE field
            /// </summary>
            DATE,
            /// <summary>
            /// Search by addCardKey field
            /// </summary>
            ADDCARD_KEY,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID
        }

        /// <summary>
        /// To return Machine Types
        /// </summary>
        public enum MachinesTypeName
        {
            TOKENPOS,
            TOKENKIOSK,
            TOKENHAND,
            TRANSFERREDTOKEN,
            CARDSONHAND,
            CARDPURCHASED
        }

       private int cardInventoryKey;
       private string fromSerialNumber;
       private string toserialNumber;
       private int number;
       private DateTime actiondate;
       private string actionBy;
       private string action;
       private string guid;
       private bool synchStatus;
       private int masterEntityId;
       private int tagType;
       private int machineType;
       private int activityType;
       private MachinesTypeName machinesTypeName;
       private DateTime lastUpdatedDate;
       private string lastUpdatedBy;
       private DateTime creationDate;
       private string createdBy;
       private int siteId;
       private string addCardKey;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TokenCardInventoryDTO()
        {
            log.LogMethodEntry();
            cardInventoryKey = -1;
            number = 0;
            siteId = -1;
            masterEntityId = -1;
            tagType = -1;
            machineType = -1;
            activityType = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public TokenCardInventoryDTO(int cardInventoryKey, string fromSerialNumber, string toserialNumber, int number, DateTime actiondate,
                    string actionBy, string action, int tagType, int machineType, int activityType, string addCardKey)
            :this()
        {
            log.LogMethodEntry(cardInventoryKey, fromSerialNumber, toserialNumber, number, actiondate,
                               actionBy, action,  tagType, machineType,
                               activityType, addCardKey);

            this.cardInventoryKey = cardInventoryKey;
            this.fromSerialNumber = fromSerialNumber;
            this.toserialNumber = toserialNumber;
            this.number = number;
            this.actiondate = actiondate;
            this.actionBy = actionBy;
            this.action = action;
            this.tagType = tagType;
            this.machineType = machineType;
            this.activityType = activityType;
            this.addCardKey = addCardKey;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TokenCardInventoryDTO(int cardInventoryKey, string fromSerialNumber, string toserialNumber, int number, DateTime actiondate,
                                     string actionBy, string action, string guid, bool synchStatus, int masterEntityId, int tagType, int machineType,
                                     int activityType, DateTime lastUpdatedDate, string lastUpdatedBy, int siteId, DateTime creationDate, string createdBy, string addCardKey)
            :this(cardInventoryKey, fromSerialNumber, toserialNumber, number, actiondate, actionBy, action, tagType, machineType,
                  activityType, addCardKey)
        {
            log.LogMethodEntry(cardInventoryKey, fromSerialNumber, toserialNumber, number, actiondate,
                               actionBy, action, guid, synchStatus, masterEntityId, tagType, machineType,
                               activityType, lastUpdatedDate, lastUpdatedBy, siteId, creationDate, createdBy, addCardKey);
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.siteId = siteId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the TokenCardInventoryCode field
        /// </summary>
        [DisplayName("TokenCardInventory Code")]
        public int cardInventoryKeyId { get { return cardInventoryKey; } set { cardInventoryKey = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FromSerialNumber field
        /// </summary>
        [DisplayName("FromSerialNumber")]
        public string FromSerialNumber { get { return fromSerialNumber; } set { fromSerialNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ToserialNumber field
        /// </summary>
        [DisplayName("ToserialNumber")]
        public string ToserialNumber { get { return toserialNumber; } set { toserialNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Number field
        /// </summary>
        [DisplayName("Number")]
        public int Number { get { return number; } set { number = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Actiondate field
        /// </summary>
        [DisplayName("Actiondate")]
        public DateTime Actiondate { get { return actiondate; } set { actiondate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActionBy field
        /// </summary>
        [DisplayName("ActionBy")]
        public string ActionBy { get { return actionBy; } set { actionBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Action field
        /// </summary>
        [DisplayName("Action")]
        public string Action { get { return action; } set { action = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>        
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

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
        /// Get/Set method of the TagType field
        /// </summary>
        [DisplayName("TagType")]
        public int TagType { get { return tagType; } set { tagType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MachineType field
        /// </summary>
        [DisplayName("MachineType")]
        public int MachineType { get { return machineType; } set { machineType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActivityType field
        /// </summary>
        [DisplayName("ActivityType")]
        public int ActivityType { get { return activityType; } set { activityType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AddCardKey field
        /// </summary>
        [DisplayName("AddCardKey")]
        public string AddCardKey { get { return addCardKey; } set { addCardKey = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MachinesTypeName field
        /// </summary>
        [DisplayName("MachinesTypeName")]
        public MachinesTypeName MachinesTypeNames { get { return machinesTypeName; } set { machinesTypeName = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || cardInventoryKey < 0;
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
