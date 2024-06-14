/********************************************************************************************
 * Project Name - Product Games Container DTO
 * Description  - Data object of Product Games Object
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
  *2.150.0     07-Mar-2022   Prajwal S              Created 
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
    public class ProductGamesContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        int cardTypeId;
        string entitlementType;
        string optionalAttribute;
        decimal? expiryTime;
        int customDataSetId;
        bool ticketAllowed;
        int? effectiveAfterDays;
        DateTime? fromDate;
        bool monday;
        bool tuesday;
        bool wednesday;
        bool thursday;
        bool friday;
        bool saturday;
        bool sunday;
        List<ProductGamesExtendedContainerDTO> productGamesExtendedContainerDTOList;
        private List<EntityOverrideDateContainerDTO> entityOverrideDateContainerDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductGamesContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProductGamesContainerDTO(int product_game_id, int product_id, int game_id, decimal? quantity, decimal? validFor, DateTime? expiryDate,
                               string validMinutesDays, int game_profile_id, string frequency, string guid,
                               int cardTypeId, string entitlementType, string optionalAttribute, decimal? expiryTime, int customDataSetId,
                               bool ticketAllowed, int? effectiveAfterDays, DateTime? fromDate, bool monday, bool tuesday,
                               bool wednesday, bool thursday, bool friday, bool saturday, bool sunday)
        {
            log.LogMethodEntry(product_game_id, product_id, game_id, quantity, validFor, expiryDate,
                               validMinutesDays, game_profile_id, frequency, guid,
                               cardTypeId, entitlementType, optionalAttribute, expiryTime, customDataSetId,
                               ticketAllowed, effectiveAfterDays, fromDate, monday, tuesday,
                               wednesday, thursday, friday, saturday, sunday);
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
            this.cardTypeId = cardTypeId;
            this.entitlementType = entitlementType;
            this.optionalAttribute = optionalAttribute;
            this.expiryTime = expiryTime;
            this.customDataSetId = customDataSetId;
            this.ticketAllowed = ticketAllowed;
            this.effectiveAfterDays = effectiveAfterDays;
            this.fromDate = fromDate;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday = saturday;
            this.sunday = sunday;
            productGamesExtendedContainerDTOList = new List<ProductGamesExtendedContainerDTO>();
            log.LogMethodExit();
        }

        public ProductGamesContainerDTO(ProductGamesContainerDTO productGamesContainerDTO)
                : this(productGamesContainerDTO.product_game_id, productGamesContainerDTO.product_id, productGamesContainerDTO.game_id, productGamesContainerDTO.quantity,
                        productGamesContainerDTO.validFor, productGamesContainerDTO.expiryDate,
                               productGamesContainerDTO.validMinutesDays, productGamesContainerDTO.game_profile_id, productGamesContainerDTO.frequency, productGamesContainerDTO.guid,
                               productGamesContainerDTO.cardTypeId, productGamesContainerDTO.entitlementType, productGamesContainerDTO.optionalAttribute, productGamesContainerDTO.expiryTime, productGamesContainerDTO.customDataSetId,
                               productGamesContainerDTO.ticketAllowed, productGamesContainerDTO.effectiveAfterDays, productGamesContainerDTO.fromDate, productGamesContainerDTO.monday, productGamesContainerDTO.tuesday,
                               productGamesContainerDTO.wednesday, productGamesContainerDTO.thursday, productGamesContainerDTO.friday, productGamesContainerDTO.saturday, productGamesContainerDTO.sunday)
        {
            log.LogMethodEntry(productGamesContainerDTO);
            if (productGamesContainerDTO.productGamesExtendedContainerDTOList != null)
            {
                productGamesExtendedContainerDTOList = new List<ProductGamesExtendedContainerDTO>();
                foreach (var productGamesExtendedContainerDTO in productGamesContainerDTO.productGamesExtendedContainerDTOList)
                {
                    ProductGamesExtendedContainerDTO copy = new ProductGamesExtendedContainerDTO(productGamesExtendedContainerDTO);
                    productGamesExtendedContainerDTOList.Add(copy);
                }
            }
            if (productGamesContainerDTO.entityOverrideDateContainerDTOList != null)
            {
                entityOverrideDateContainerDTOList = new List<EntityOverrideDateContainerDTO>();
                foreach (var entityOverrideDateContainerDTO in productGamesContainerDTO.entityOverrideDateContainerDTOList)
                {
                    EntityOverrideDateContainerDTO copy = new EntityOverrideDateContainerDTO(entityOverrideDateContainerDTO);
                    entityOverrideDateContainerDTOList.Add(copy);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Product_game_id field
        /// </summary>
        public int Product_game_id
        {
            get
            {
                return product_game_id;
            }
            set
            {
                product_game_id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Product_id field
        /// </summary>
        public int Product_id
        {
            get
            {
                return product_id;
            }
            set
            {
                product_id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Game_id field
        /// </summary>
        public int Game_id
        {
            get
            {
                return game_id;
            }
            set
            {
                game_id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        public decimal? Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                quantity = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ValidFor field
        /// </summary>
        public decimal? ValidFor
        {
            get
            {
                return validFor;
            }
            set
            {
                validFor = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        public DateTime? ExpiryDate
        {
            get
            {
                return expiryDate;
            }
            set
            {
                expiryDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ValidMinutesDays field
        /// </summary>
        public string ValidMinutesDays
        {
            get
            {
                return validMinutesDays;
            }
            set
            {
                validMinutesDays = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ValidMinutesDays field
        /// </summary>
        public int Game_profile_id
        {
            get
            {
                return game_profile_id;
            }
            set
            {
                game_profile_id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Frequency field
        /// </summary>
        public string Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                frequency = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }

        }
        /// <summary>
        /// Get/Set method of the CardTypeId field
        /// </summary>
        public int CardTypeId
        {
            get
            {
                return cardTypeId;
            }
            set
            {
                cardTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the EntitlementType field
        /// </summary>
        public string EntitlementType
        {
            get
            {
                return entitlementType;
            }
            set
            {
                entitlementType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the OptionalAttribute field
        /// </summary>
        public string OptionalAttribute
        {
            get
            {
                return optionalAttribute;
            }
            set
            {
                optionalAttribute = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ExpiryTime field
        /// </summary>
        public decimal? ExpiryTime
        {
            get
            {
                return expiryTime;
            }
            set
            {
                expiryTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CustomDataSetId field
        /// </summary>
        public int CustomDataSetId
        {
            get
            {
                return customDataSetId;
            }
            set
            {
                customDataSetId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TicketAllowed field
        /// </summary>
        public bool TicketAllowed
        {
            get
            {
                return ticketAllowed;
            }
            set
            {
                ticketAllowed = value;
            }
        }

        /// <summary>
        /// Get/Set method of the EffectiveAfterDays field
        /// </summary>
        public int? EffectiveAfterDays
        {
            get
            {
                return effectiveAfterDays;
            }
            set
            {
                effectiveAfterDays = value;
            }
        }

        /// <summary>
        /// Get/Set method of the FromDate field
        /// </summary>
        public DateTime? FromDate
        {
            get
            {
                return fromDate;
            }
            set
            {
                fromDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Monday field
        /// </summary>
        public bool Monday
        {
            get
            {
                return monday;
            }
            set
            {
                monday = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Tuesday field
        /// </summary>
        public bool Tuesday
        {
            get
            {
                return tuesday;
            }
            set
            {
                tuesday = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Wednesday field
        /// </summary>
        public bool Wednesday
        {
            get
            {
                return wednesday;
            }
            set
            {
                wednesday = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Thursday field
        /// </summary>
        public bool Thursday
        {
            get
            {
                return thursday;
            }
            set
            {
                thursday = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Friday field
        /// </summary>
        public bool Friday
        {
            get
            {
                return friday;
            }
            set
            {
                friday = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Saturday field
        /// </summary>
        public bool Saturday
        {
            get
            {
                return saturday;
            }
            set
            {
                saturday = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Sunday field
        /// </summary>
        public bool Sunday
        {
            get
            {
                return sunday;
            }
            set
            {
                sunday = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ProductGamesExtendedContainerDTOList field
        /// </summary>
        public List<ProductGamesExtendedContainerDTO> ProductGamesExtendedContainerDTOList
        {
            get
            {
                return productGamesExtendedContainerDTOList;
            }

            set
            {
                productGamesExtendedContainerDTOList = value;
            }
        }


        /// <summary>
        /// Get/Set for EntityOverrideDateContainerDTOList Field
        /// </summary>
        public List<EntityOverrideDateContainerDTO> EntityOverrideDateContainerDTOList { get { return entityOverrideDateContainerDTOList; } set { entityOverrideDateContainerDTOList = value; } }
    }



}
