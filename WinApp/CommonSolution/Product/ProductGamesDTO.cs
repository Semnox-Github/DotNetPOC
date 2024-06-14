/********************************************************************************************
 * Project Name - Product Games DTO
 * Description  - Data object of Product Games Object
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        31-Jan-2019   Akshay Gulaganji          Created 
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the Product Games data object class. This acts as data holder for the Product Games business object
    /// </summary>
    public class ProductGamesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        ///// <summary>
        ///// SearchByProductGamesParameters enum controls the search fields, this can be expanded to include additional fields
        ///// </summary>
        public enum SearchByProductGamesParameters
        {
            /// <summary>
            /// Search by PRODUCT_ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by PRODUCT_GAME_ID field
            /// </summary>
            PRODUCT_GAME_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by PRODUCT_ID_LIST field
            /// </summary>
            PRODUCT_ID_LIST,
            /// <summary>
            /// Search by GAME_ID field
            /// </summary>
            GAME_ID,
            /// <summary>
            /// Search by GAME_PROFILE_ID field
            /// </summary>
            GAME_PROFILE_ID,
        }

        int product_game_id;
        int product_id;
        int game_id;
        decimal? quantity;
        decimal? validFor;
        DateTime? expiryDate;
        string validMinutesDays;
        int game_profile_id;
        string frequency;
        string guid;
        int site_id;
        bool synchStatus;
        int cardTypeId;
        string entitlementType;
        string optionalAttribute;
        decimal? expiryTime;
        int customDataSetId;
        bool ticketAllowed;
        int? effectiveAfterDays;
        DateTime? fromDate;
        int masterEntityId;
        bool monday;
        bool tuesday;
        bool wednesday;
        bool thursday;
        bool friday;
        bool saturday;
        bool sunday;
        string createdBy;
        DateTime? creationDate;
        string lastUpdatedBy;
        DateTime? lastUpdateDate;
        bool isActive;
        SortableBindingList<ProductGamesExtendedDTO> productGamesExtendedDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductGamesDTO()
        {
            log.LogMethodEntry();
            product_game_id=-1;
            product_id=-1;
            game_id=-1;
            validMinutesDays="D";
            game_profile_id=-1;
            frequency="N";
            cardTypeId=-1;
            customDataSetId=-1;
            ticketAllowed=true;
            monday=true;
            tuesday = true;
            wednesday = true;
            thursday = true;
            friday = true;
            saturday = true;
            sunday = true;
            isActive=true;
            productGamesExtendedDTOList = new SortableBindingList<ProductGamesExtendedDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        /// <param name="product_game_id"></param>
        /// <param name="product_id"></param>
        /// <param name="game_id"></param>
        /// <param name="quantity"></param>
        /// <param name="validFor"></param>
        /// <param name="expiryDate"></param>
        /// <param name="validMinutesDays"></param>
        /// <param name="game_profile_id"></param>
        /// <param name="frequency"></param>
        /// <param name="guid"></param>
        /// <param name="site_id"></param>
        /// <param name="synchStatus"></param>
        /// <param name="cardTypeId"></param>
        /// <param name="entitlementType"></param>
        /// <param name="optionalAttribute"></param>
        /// <param name="expiryTime"></param>
        /// <param name="customDataSetId"></param>
        /// <param name="ticketAllowed"></param>
        /// <param name="effectiveAfterDays"></param>
        /// <param name="fromDate"></param>
        /// <param name="masterEntityId"></param>
        /// <param name="monday"></param>
        /// <param name="tuesday"></param>
        /// <param name="wednesday"></param>
        /// <param name="thursday"></param>
        /// <param name="friday"></param>
        /// <param name="saturday"></param>
        /// <param name="sunday"></param>
        /// <param name="createdBy"></param>
        /// <param name="creationDate"></param>
        /// <param name="lastUpdatedBy"></param>
        /// <param name="lastUpdateDate"></param>
        /// <param name="isActive"></param>
        public ProductGamesDTO(int product_game_id, int product_id, int game_id, decimal? quantity, decimal? validFor, DateTime? expiryDate,
                               string validMinutesDays, int game_profile_id, string frequency, string guid, int site_id, bool synchStatus,
                               int cardTypeId, string entitlementType, string optionalAttribute, decimal? expiryTime, int customDataSetId,
                               bool ticketAllowed, int? effectiveAfterDays, DateTime? fromDate, int masterEntityId, bool monday, bool tuesday,
                               bool wednesday, bool thursday, bool friday, bool saturday, bool sunday, string createdBy, DateTime? creationDate,
                               string lastUpdatedBy, DateTime? lastUpdateDate,bool isActive)
        {
            log.LogMethodEntry(product_game_id, product_id, game_id, quantity, validFor, expiryDate,
                               validMinutesDays, game_profile_id, frequency, guid, site_id, synchStatus,
                               cardTypeId, entitlementType, optionalAttribute, expiryTime, customDataSetId,
                               ticketAllowed, effectiveAfterDays, fromDate, masterEntityId, monday, tuesday,
                               wednesday, thursday, friday, saturday, sunday, createdBy, creationDate,
                               lastUpdatedBy, lastUpdateDate, isActive);
            this.product_game_id = product_game_id;
            this.product_id = product_id;
            this.game_id = game_id;
            this.quantity = quantity;
            this.validFor = validFor;
            this.expiryDate = expiryDate;
            this.validMinutesDays = validMinutesDays;
            this.game_profile_id = game_profile_id;
            this.frequency = frequency;
            this.guid = guid;
            this.site_id = site_id;
            this.synchStatus = synchStatus;
            this.cardTypeId = cardTypeId;
            this.entitlementType = entitlementType;
            this.optionalAttribute = optionalAttribute;
            this.expiryTime = expiryTime;
            this.customDataSetId = customDataSetId;
            this.ticketAllowed = ticketAllowed;
            this.effectiveAfterDays = effectiveAfterDays;
            this.fromDate = fromDate;
            this.masterEntityId = masterEntityId;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday = saturday;
            this.sunday = sunday;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.isActive = isActive;
            productGamesExtendedDTOList = new SortableBindingList<ProductGamesExtendedDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Product_game_id field
        /// </summary>
        [DisplayName("ID")]
        [ReadOnly(true)]
        public int Product_game_id
        {
            get
            {
                return product_game_id;
            }
            set
            {
                this.IsChanged = true;
                product_game_id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Product_id field
        /// </summary>
        [DisplayName("Product ID")]
        [ReadOnly(false)]
        public int Product_id
        {
            get
            {
                return product_id;
            }
            set
            {
                this.IsChanged = true;
                product_id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Game_id field
        /// </summary>
        [DisplayName("Game")]
        [ReadOnly(true)]
        public int Game_id
        {
            get
            {
                return game_id;
            }
            set
            {
                this.IsChanged = true;
                game_id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        [DisplayName("Play Count / Entt. Value")]
        [ReadOnly(true)]
        public decimal? Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                this.IsChanged = true;
                quantity = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ValidFor field
        /// </summary>
        [DisplayName("Valid For")]
        [ReadOnly(true)]
        public decimal? ValidFor
        {
            get
            {
                return validFor;
            }
            set
            {
                this.IsChanged = true;
                validFor = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        [DisplayName("Expiry Date")]
        [ReadOnly(true)]
        public DateTime? ExpiryDate
        {
            get
            {
                return expiryDate;
            }
            set
            {
                this.IsChanged = true;
                expiryDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ValidMinutesDays field
        /// </summary>
        [DisplayName("Valid For Days / Minutes")]
        [ReadOnly(true)]
        public string ValidMinutesDays
        {
            get
            {
                return validMinutesDays;
            }
            set
            {
                this.IsChanged = true;
                validMinutesDays = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ValidMinutesDays field
        /// </summary>
        [DisplayName("Valid For Days / Minutes")]
        [ReadOnly(true)]
        public int Game_profile_id
        {
            get
            {
                return game_profile_id;
            }
            set
            {
                this.IsChanged = true;
                game_profile_id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Frequency field
        /// </summary>
        [DisplayName("Frequency")]
        [ReadOnly(true)]
        public string Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                this.IsChanged = true;
                frequency = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [ReadOnly(false)]
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                this.guid = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site Id")]
        [ReadOnly(false)]
        public int Site_id
        {
            get
            {
                return site_id;
            }
            set
            {
                this.site_id = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Site Id")]
        [ReadOnly(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                this.IsChanged = true;
                synchStatus = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CardTypeId field
        /// </summary>
        [DisplayName("Card Type Id")]
        [ReadOnly(false)]
        public int CardTypeId
        {
            get
            {
                return cardTypeId;
            }
            set
            {
                this.IsChanged = true;
                cardTypeId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the EntitlementType field
        /// </summary>
        [DisplayName("Entitlement Type")]
        [ReadOnly(true)]
        public string EntitlementType
        {
            get
            {
                return entitlementType;
            }
            set
            {
                this.IsChanged = true;
                entitlementType = value;
            }
        }
        /// <summary>
        /// Get/Set method of the OptionalAttribute field
        /// </summary>
        [DisplayName("Optional Attribute")]
        [ReadOnly(true)]
        public string OptionalAttribute
        {
            get
            {
                return optionalAttribute;
            }
            set
            {
                this.IsChanged = true;
                optionalAttribute = value;
            }
        }
        /// <summary>
        /// Get/Set method of the ExpiryTime field
        /// </summary>
        [DisplayName("Expiry Time")]
        [ReadOnly(true)]
        public decimal? ExpiryTime
        {
            get
            {
                return expiryTime;
            }
            set
            {
                this.IsChanged = true;
                expiryTime = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CustomDataSetId field
        /// </summary>
        [DisplayName("Custom DataSet Id")]
        [ReadOnly(false)]
        public int CustomDataSetId
        {
            get
            {
                return customDataSetId;
            }
            set
            {
                this.IsChanged = true;
                customDataSetId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the TicketAllowed field
        /// </summary>
        [DisplayName("Ticket Allowed")]
        [ReadOnly(true)]
        public bool TicketAllowed
        {
            get
            {
                return ticketAllowed;
            }
            set
            {
                this.IsChanged = true;
                ticketAllowed = value;
            }
        }
        /// <summary>
        /// Get/Set method of the EffectiveAfterDays field
        /// </summary>
        [DisplayName("Effective After Days")]
        [ReadOnly(true)]
        public int? EffectiveAfterDays
        {
            get
            {
                return effectiveAfterDays;
            }
            set
            {
                this.IsChanged = true;
                effectiveAfterDays = value;
            }
        }
        /// <summary>
        /// Get/Set method of the FromDate field
        /// </summary>
        [DisplayName("From Date")]
        [ReadOnly(true)]
        public DateTime? FromDate
        {
            get
            {
                return fromDate;
            }
            set
            {
                this.IsChanged = true;
                fromDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [ReadOnly(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Monday field
        /// </summary>
        [DisplayName("Monday")]
        [ReadOnly(true)]
        public bool Monday
        {
            get
            {
                return monday;
            }
            set
            {
                this.IsChanged = true;
                monday = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Tuesday field
        /// </summary>
        [DisplayName("Tuesday")]
        [ReadOnly(true)]
        public bool Tuesday
        {
            get
            {
                return tuesday;
            }
            set
            {
                this.IsChanged = true;
                tuesday = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Wednesday field
        /// </summary>
        [DisplayName("Wednesday")]
        [ReadOnly(true)]
        public bool Wednesday
        {
            get
            {
                return wednesday;
            }
            set
            {
                this.IsChanged = true;
                wednesday = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Thursday field
        /// </summary>
        [DisplayName("Thursday")]
        [ReadOnly(true)]
        public bool Thursday
        {
            get
            {
                return thursday;
            }
            set
            {
                this.IsChanged = true;
                thursday = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Friday field
        /// </summary>
        [DisplayName("Friday")]
        [ReadOnly(true)]
        public bool Friday
        {
            get
            {
                return friday;
            }
            set
            {
                this.IsChanged = true;
                friday = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Saturday field
        /// </summary>
        [DisplayName("Saturday")]
        [ReadOnly(true)]
        public bool Saturday
        {
            get
            {
                return saturday;
            }
            set
            {
                this.IsChanged = true;
                saturday = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Sunday field
        /// </summary>
        [DisplayName("Sunday")]
        [ReadOnly(true)]
        public bool Sunday
        {
            get
            {
                return sunday;
            }
            set
            {
                this.IsChanged = true;
                sunday = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        [ReadOnly(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                this.IsChanged = true;
                createdBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        [ReadOnly(false)]
        public DateTime? CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                this.IsChanged = true;
                creationDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        [ReadOnly(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                this.IsChanged = true;
                lastUpdatedBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        [ReadOnly(false)]
        public DateTime? LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {
                this.IsChanged = true;
                lastUpdateDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the ISActive field
        /// </summary>
        [DisplayName("ISActive")]
        [ReadOnly(true)]
        public bool ISActive
        {
            get
            {
                return isActive;
            }
            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }
        /// <summary>
        /// Get/Set method of the ProductGamesExtendedDTOList field
        /// </summary>
        [Browsable(false)]
        public SortableBindingList<ProductGamesExtendedDTO> ProductGamesExtendedDTOList
        {
            get
            {
                return productGamesExtendedDTOList;
            }

            set
            {
                productGamesExtendedDTOList = value;
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
                    return notifyingObjectIsChanged || product_game_id < 0;
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
            log.LogMethodExit(null);
        }
    }



}
