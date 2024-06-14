/*/********************************************************************************************
 * Project Name - LegacyCardCreditPlusConsumptionDTO
 * Description  - Data Object File for LegacyCardCreditPlusConsumptionDTO
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
    /// This is the LegacyCardCreditPlusConsumptionDTO data object class. This acts as data holder for the LegacyCardCreditPlus business objects
    /// </summary>
    public class LegacyCardCreditPlusConsumptionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Search By LegacyCardCreditPlus enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by legacyCardCreditPlusConsumptionId field
            /// </summary>
            LEGACY_CARDCREDIT_PLUS_CONSUMPTION_ID,
            /// <summary>
            /// Search by Legacy Card Credit Plus Id field
            /// </summary>
            LEGACY_CARD_CREDIT_PLUS_ID,
            /// <summary>
            /// Search by Pos Counter Name field
            /// </summary>
            POS_COUNTER_NAME,
            /// <summary>
            /// Search by Card Id List field
            /// </summary>
            CARD_ID_LIST,
            /// <summary>
            /// Search by Site Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int legacyCardCreditPlusConsumptionId;
        private int legacyCardCreditPlusId;
        private string posCounterName;
        private DateTime? expiryDate;
        private string productName;
        private string gameName;
        private string gameProfileName;
        private decimal? discountPercentage;
        private decimal? discountedPrice;
        private int? consumptionBalance;
        private int? quantityLimit;
        private string categoryname;
        private decimal? discountAmount;
        private int? consumptionQty;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastupdatedDate;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;


        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor of LegacyCardCreditPlusConsumptionDTO with required fields
        /// </summary>
        public LegacyCardCreditPlusConsumptionDTO()
        {
            log.LogMethodEntry();
            legacyCardCreditPlusConsumptionId = -1;
            legacyCardCreditPlusId = -1;
            site_id = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of LegacyCardCreditPlusConsumptionDTO with the required fields
        /// </summary>
        public LegacyCardCreditPlusConsumptionDTO(int legacyCardCreditPlusConsumptionId, int legacyCardCreditPlusId, string posCounterName, DateTime? expiryDate, string productName,
                                                  string gameName, string gameProfileName, decimal? discountPercentage, decimal? discountedPrice, int? consumptionBalance, int? quantityLimit, string categoryname, decimal? discountAmount,
                                                  int? consumptionQty)
            : this()
        {
            log.LogMethodEntry(legacyCardCreditPlusConsumptionId, legacyCardCreditPlusId, posCounterName, expiryDate, productName, gameName, gameProfileName, discountPercentage, discountedPrice,
                 consumptionBalance, quantityLimit, categoryname, discountAmount, consumptionQty);
            this.legacyCardCreditPlusConsumptionId = legacyCardCreditPlusConsumptionId;
            this.legacyCardCreditPlusId = legacyCardCreditPlusId;
            this.posCounterName = posCounterName;
            this.expiryDate = expiryDate;
            this.expiryDate = expiryDate;
            this.productName = productName;
            this.gameName = gameName;
            this.gameProfileName = gameProfileName;
            this.discountPercentage = discountPercentage;
            this.discountedPrice = discountedPrice;
            this.consumptionBalance = consumptionBalance;
            this.quantityLimit = quantityLimit;
            this.categoryname = categoryname;
            this.discountAmount = discountAmount;
            this.consumptionQty = consumptionQty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of LegacyCardCreditPlusConsumptionDTO with all fields
        /// </summary>
        public LegacyCardCreditPlusConsumptionDTO(int legacyCardCreditPlusConsumptionId, int legacyCardCreditPlusId, string posCounterName, DateTime? expiryDate, string productName,
                                                  string gameName, string gameProfileName, decimal? discountPercentage, decimal? discountedPrice, int? consumptionBalance, int? quantityLimit, string categoryname, decimal? discountAmount,
                                                  int? consumptionQty, string createdBy, DateTime creationDate, string lastUpdatedBy, bool isActive, DateTime lastupdatedDate,
                                  int site_id, string guid, bool synchStatus, int masterEntityId)
        : this(legacyCardCreditPlusConsumptionId, legacyCardCreditPlusId, posCounterName, expiryDate, productName, gameName, gameProfileName, discountPercentage, discountedPrice,
                 consumptionBalance, quantityLimit, categoryname, discountAmount, consumptionQty)
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
        /// Get/Set method of the LegacyCardCreditPlusConsumptionId field
        /// </summary>
        public int LegacyCardCreditPlusConsumptionId { get { return legacyCardCreditPlusConsumptionId; } set { legacyCardCreditPlusConsumptionId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LegacyCardCreditPlusId field
        /// </summary>
        public int LegacyCardCreditPlusId { get { return legacyCardCreditPlusId; } set { legacyCardCreditPlusId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PosCounterName field
        /// </summary>
        public string PosCounterName { get { return posCounterName; } set { posCounterName = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        public DateTime? ExpiryDate { get { return expiryDate; } set { expiryDate = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductName field
        /// </summary>
        public string ProductName { get { return productName; } set { productName = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the GameName field
        /// </summary>
        public string GameName { get { return gameName; } set { gameName = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the GameProfileName field
        /// </summary>
        public string GameProfileName { get { return gameProfileName; } set { gameProfileName = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DiscountPercentage field
        /// </summary>
        public decimal? DiscountPercentage { get { return discountPercentage; } set { discountPercentage = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DiscountedPrice field
        /// </summary>
        public decimal? DiscountedPrice { get { return discountedPrice; } set { discountedPrice = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ConsumptionBalance field
        /// </summary>
        public int? ConsumptionBalance { get { return consumptionBalance; } set { consumptionBalance = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the QuantityLimit field
        /// </summary>
        public int? QuantityLimit { get { return quantityLimit; } set { quantityLimit = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Categoryname field
        /// </summary>
        public string Categoryname { get { return categoryname; } set { categoryname = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DiscountAmount field
        /// </summary>
        public decimal? DiscountAmount { get { return discountAmount; } set { discountAmount = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ConsumptionQty field
        /// </summary>
        public int? ConsumptionQty { get { return consumptionQty; } set { consumptionQty = value; IsChanged = true; } }
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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || LegacyCardCreditPlusConsumptionId < 0;
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
