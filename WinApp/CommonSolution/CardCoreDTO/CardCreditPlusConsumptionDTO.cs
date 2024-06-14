/********************************************************************************************
 * Project Name - CardCore project 
 * Description  - Data object of the CardCreditPlusConsumptionDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00       7-Nov-2017     Jeevan           Created 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.CardCore
{
    /// <summary>
    /// CardCreditPlusConsumptionDTO Class
    /// </summary>
    public class CardCreditPlusConsumptionDTO
    {

        int pKId;
        int cardCreditPlusId;
        int pOSTypeId;
        DateTime expiryDate;
        int productId;
        int gameProfileId;
        int gameId;
        double discountedPrice;
        decimal discountPercentage;
        decimal discountAmount;
        int consumptionBalance;
        int quantityLimit;
        int categoryId;
        int site_id;
        string guid;
        string lastUpdatedBy;
        DateTime lastupdatedDate;
        bool synchStatus;
        int masterEntityId;

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        bool notifyingObjectIsChanged;
        readonly object notifyingObjectIsChangedSyncRoot = new Object();

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by PKId field
            /// </summary>
            PKId = 0,
            /// <summary>
            /// Search by CardCreditPlusId field
            /// </summary>
            CARD_CREDIT_PLUS_ID = 1,

        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CardCreditPlusConsumptionDTO()
        {
            this.pKId = -1;
            this.cardCreditPlusId = -1;
            this.pOSTypeId = -1;
            this.site_id = -1;


        }


        /// <summary>
        /// Parameterized Contructor
        /// </summary>
        public CardCreditPlusConsumptionDTO(int pKId, int cardCreditPlusId, int pOSTypeId, DateTime expiryDate,
                           int productId, int gameProfileId, int gameId, 
                           double discountedPrice, decimal discountPercentage, decimal discountAmount,
                           int consumptionBalance, int quantityLimit, int categoryId, 
                           int site_id, string guid,  string lastUpdatedBy, DateTime lastupdatedDate,
                           bool synchStatus,  int masterEntityId)
        {


            this.pKId = pKId;
            this.cardCreditPlusId = cardCreditPlusId;
            this.pOSTypeId = pOSTypeId;
            this.expiryDate = expiryDate;
            this.productId = productId;
            this.gameProfileId = gameProfileId;
            this.gameId = gameId;
            this.discountedPrice = discountedPrice;
            this.discountPercentage = discountPercentage;
            this.discountAmount = discountAmount;
            this.consumptionBalance = consumptionBalance;
            this.quantityLimit = quantityLimit;
            this.categoryId = categoryId;
            this.site_id = site_id;
            this.guid = guid;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastupdatedDate = lastupdatedDate;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
        }


        [DefaultValue(-1)]
        public int PKId { get { return pKId; } set { pKId = value; this.IsChanged = true; } }

        public int CardCreditPlusId { get { return cardCreditPlusId; } set { cardCreditPlusId = value; this.IsChanged = true; } }
        public int POSTypeId { get { return pOSTypeId; } set { pOSTypeId = value; this.IsChanged = true; } }
        public DateTime ExpiryDate { get { return expiryDate; } set { expiryDate = value; this.IsChanged = true; } }
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }
        public int GameProfileId { get { return gameProfileId; } set { gameProfileId = value; this.IsChanged = true; } }
        public int GameId { get { return gameId; } set { gameId = value; this.IsChanged = true; } }
        public decimal DiscountPercentage { get { return discountPercentage; } set { discountPercentage = value; this.IsChanged = true; } }
        public decimal DiscountAmount { get { return discountAmount; } set { discountAmount = value; this.IsChanged = true; } }
        public int CategoryId { get { return categoryId; } set { categoryId = value; this.IsChanged = true; } }
        public int ConsumptionBalance { get { return consumptionBalance; } set { consumptionBalance = value; this.IsChanged = true; } }
        public int QuantityLimit { get { return quantityLimit; } set { quantityLimit = value; this.IsChanged = true; } }

        public int SiteId { get { return site_id; } set { site_id = value; } }
        public string Guid { get { return guid; } set { guid = value; } }
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        public DateTime LastUpdatedDate { get { return lastupdatedDate; } set { lastupdatedDate = value; } }
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || pKId < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }


    }
}
