/*/********************************************************************************************
 * Project Name - LegacyCardGamesDTO
 * Description  - Data Object File for LegacyCardGamesDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By             Remarks 
 *********************************************************************************************
 *2.130.4     18-Feb-2022    Dakshakh                Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parafait_POS
{
    /// <summary>
    /// This is the LegacyCardGamesDTO data object class. This acts as data holder for the LegacyCardCreditPlus business objects
    /// </summary>
    public class LegacyCardGamesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Search By LegacyCardCreditPlus enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by Legacy Card Game Id field
            /// </summary>
            LEGACY_CARD_GAME_ID,
            /// <summary>
            /// Search by Legacy game name field
            /// </summary>
            LEGACY_GAME_NAME,
            /// <summary>
            /// Search by LegacyCard_id field
            /// </summary>
            LEGACY_CARD_ID,
            /// <summary>
            /// Search by Site Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by CARD_ID_LIST field
            /// </summary>
            CARD_ID_LIST,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int legacyCardGameId;
        private int legacyCard_id;
        private string legacycardGame_name;
        private decimal quantity;
        private decimal revisedQuantity;
        private DateTime? expiryDate;
        private string gameProfileName;
        private string frequency;
        private bool ticketAllowed;
        private DateTime? fromDate;
        private bool monday;
        private bool tuesday;
        private bool wednesday;
        private bool thursday;
        private bool friday;
        private bool saturday;
        private bool sunday;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private bool isActive;
        private DateTime lastupdatedDate;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;

        private List<LegacyCardGameExtendedDTO> legacyCardGameExtendedDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor of LegacyCardGamesDTO with required fields
        /// </summary>
        public LegacyCardGamesDTO()
        {
            log.LogMethodEntry();
            legacyCardGameId = -1;
            LegacyCard_id = -1;
            site_id = -1;
            masterEntityId = -1;
            isActive = true;
            frequency = "N";
            sunday = true;
            monday = true;
            tuesday = true;
            wednesday = true;
            thursday = true;
            friday = true;
            saturday = true;
            ticketAllowed = true;
            legacyCardGameExtendedDTOList = new List<LegacyCardGameExtendedDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of LegacyCardGamesDTO with the required fields
        /// </summary>
        public LegacyCardGamesDTO(int legacyCardGameId, int legacyCard_id, string legacycardGame_name, decimal quantity,decimal revisedQuantity, DateTime? expiryDate, string gameProfileName, 
                                  string frequency, bool ticketAllowed, DateTime? fromDate, bool monday, bool tuesday, bool wednesday, bool thursday, bool friday,
                                  bool saturday, bool sunday)
            : this()
        {
            log.LogMethodEntry(legacyCardGameId, legacyCard_id, legacycardGame_name, quantity, revisedQuantity, expiryDate, gameProfileName, frequency, ticketAllowed, fromDate, monday, 
                               tuesday, wednesday, thursday, friday, saturday, sunday);
            this.legacyCardGameId = legacyCardGameId;
            this.legacyCard_id = legacyCard_id;
            this.legacycardGame_name = legacycardGame_name;
            this.quantity = quantity;
            this.revisedQuantity = revisedQuantity;
            this.expiryDate = expiryDate;
            this.gameProfileName = gameProfileName;
            this.frequency = frequency;
            this.ticketAllowed = ticketAllowed;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday = saturday;
            this.sunday = sunday;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of LegacyCardGamesDTO with all fields
        /// </summary>
        public LegacyCardGamesDTO(int legacyCardGameId, int legacyCard_id, string legacycardGame_name, decimal quantity, decimal revisedQuantity, DateTime? expiryDate, string game_profile_name,
                                  string frequency,  bool ticketAllowed, DateTime? fromDate, bool monday, bool tuesday, bool wednesday, bool thursday, bool friday,
                                  bool saturday, bool sunday, string createdBy, DateTime creationDate, string lastUpdatedBy, bool isActive, DateTime lastupdatedDate, 
                                  int site_id, string guid, bool synchStatus, int masterEntityId)
        : this(legacyCardGameId, legacyCard_id, legacycardGame_name, quantity, revisedQuantity, expiryDate, game_profile_name, frequency, ticketAllowed, fromDate, monday,
                               tuesday, wednesday, thursday, friday, saturday, sunday)
        {
            log.LogMethodEntry(lastupdatedDate, site_id, lastUpdatedBy, guid, synchStatus, masterEntityId, isActive);
            this.lastupdatedDate = lastupdatedDate;
            this.site_id = site_id;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LegacyCardGameId field
        /// </summary>
        public int LegacyCardGameId { get { return legacyCardGameId; } set { legacyCardGameId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LegacyCard_id field
        /// </summary>
        public int LegacyCard_id { get { return legacyCard_id; } set { legacyCard_id = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LegacycardGame_name field
        /// </summary>
        public string LegacycardGame_name { get { return legacycardGame_name; } set { legacycardGame_name = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        public decimal Quantity { get { return quantity; } set { quantity = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the revisedQuantity field
        /// </summary>
        public decimal RevisedQuantity { get { return revisedQuantity; } set { revisedQuantity = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Game_profile_name field
        /// </summary>
        public string GameProfileName { get { return gameProfileName; } set { gameProfileName = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Frequency field
        /// </summary>
        public string Frequency { get { return frequency; } set { frequency = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TicketAllowed field
        /// </summary>
        public bool TicketAllowed { get { return ticketAllowed; } set { ticketAllowed = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the FromDate field
        /// </summary>
        public DateTime? FromDate { get { return fromDate; } set { fromDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the FromDate field
        /// </summary>
        public DateTime? ExpiryDate { get { return expiryDate; } set { expiryDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Monday field
        /// </summary>
        public bool Monday { get { return monday; } set { monday = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Tuesday field
        /// </summary>
        public bool Tuesday { get { return tuesday; } set { tuesday = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Wednesday field
        /// </summary>
        public bool Wednesday { get { return wednesday; } set { wednesday = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Thursday field
        /// </summary>
        public bool Thursday { get { return thursday; } set { thursday = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the friday field
        /// </summary>
        public bool Friday { get { return friday; } set { friday = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Saturday field
        /// </summary>
        public bool Saturday { get { return saturday; } set { saturday = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Sunday field
        /// </summary>
        public bool Sunday { get { return sunday; } set { sunday = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastupdatedDate; } set { lastupdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        public int Site_id { get { return site_id; } set { site_id = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set Methods for LegacyCardCreditPlusDTOList field
        /// </summary>
        public List<LegacyCardGameExtendedDTO> LegacyCardGameExtendedDTOList
        {
            get
            {
                return legacyCardGameExtendedDTOList;
            }
            set
            {
                legacyCardGameExtendedDTOList = value;
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
                    return notifyingObjectIsChanged || LegacyCardGameId < 0;
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
        /// Returns whether any child record is changed
        /// </summary>
        [Browsable(false)]
        public bool IsChangedRecursive
        {
            get
            {
                bool isChangedRecursive = IsChanged;
                if (legacyCardGameExtendedDTOList != null)
                {
                    foreach (var legacyCardGameExtendedDTO in legacyCardGameExtendedDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || legacyCardGameExtendedDTO.IsChanged;
                    }
                }
                return isChangedRecursive;
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
