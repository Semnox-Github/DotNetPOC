/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data object of PromotionDetails
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80        24-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// This is the PromotionDetail data object class. This acts as data holder for the PromotionDetail business object
    /// </summary>
    public class PromotionDetailDTO
    { 

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by    PROMOTION_DETAIL ID field
            /// </summary>
            PROMOTION_DETAIL_ID,
            /// <summary>
            /// Search by    PROMOTION ID field
            /// </summary>
            PROMOTION_ID,
            /// <summary>
            /// Search by    PROMOTION ID LIST field
            /// </summary>
            PROMOTION_ID_LIST,
            /// <summary>
            /// Search by GAME ID field
            /// </summary>
            GAME_ID,
            /// <summary>
            /// Search by  GAME PROFILE ID field
            /// </summary>
            GAME_PROFILE_ID,
            /// <summary>
            /// Search by PRODUCT ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by  CATEGORY ID field
            /// </summary>
            CATEGORY_ID,
            /// <summary>
            /// Search by  THEME ID field
            /// </summary>
            THEME_ID,
            /// <summary>
            /// Search by  VISUALIZATION THEME ID field
            /// </summary>
            VISUALIZATION_THEME_ID,
            /// <summary>
            /// Search by  BONUS ALLOWED field
            /// </summary>
            BONUS_ALLOWED,
            /// <summary>
            /// Search by  COURTESY ALLOWED field
            /// </summary>
            COURTESY_ALLOWED,
            /// <summary>
            /// Search by  TICKETS ALLOWED field
            /// </summary>
            TICKETS_ALLOWED,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID
        }
        private int promotionDetailId;
        private int promotionId;
        private int gameId;
        private int gameprofileId;
        private decimal? absoluteCredits;
        private decimal? absoluteVIPCredits;
        private decimal? discountOnCredits;
        private decimal? discountOnVIPCredits;
        private char? bonusAllowed;
        private char? courtesyAllowed;
        private char? timeAllowed;
        private char? ticketAllowed;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int? internetKey;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int productId;
        private int categoryId;
        private int masterEntityId;
        private int themeId;
        private decimal? discountAmount;
        private int visualizationThemeId;
        private string createdBy;
        private DateTime creationDate;
        private bool isActive;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();


        /// <summary>
        /// Default Constructor
        /// </summary>
        public PromotionDetailDTO()
        {
            log.LogMethodEntry();
            promotionDetailId = -1;
            promotionId = -1;
            gameId = -1;
            gameprofileId = -1;
            productId = -1;
            categoryId = -1;
            promotionId = -1;
            themeId = -1;
            visualizationThemeId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            absoluteCredits = null;
            absoluteVIPCredits = null;
            discountOnCredits = null;
            discountOnVIPCredits =null;
            bonusAllowed = null;
            courtesyAllowed = null;
            timeAllowed = null;
            ticketAllowed =null;
            internetKey = null;
            discountAmount = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required fields
        /// </summary>
        public PromotionDetailDTO(int promotionDetailId, int promotionId,int gameId,int gameprofileId,decimal? absoluteCredits,
                                   decimal? absoluteVIPCredits,decimal? discountOnCredits, decimal? discountOnVIPCredits,
                                   char? bonusAllowed,char? courtesyAllowed,char? timeAllowed,char? ticketAllowed, int? internetKey, 
                                   int productId, int categoryId, int themeId,decimal? discountAmount,int visualizationThemeId, bool isActive) 
            : this()
        {

            log.LogMethodEntry(promotionDetailId, promotionId, gameId, gameprofileId, absoluteCredits,absoluteVIPCredits, 
                              discountOnCredits, discountOnVIPCredits,bonusAllowed, courtesyAllowed, timeAllowed, ticketAllowed,
                              internetKey, productId,categoryId, themeId, discountAmount, visualizationThemeId, isActive);
            this.promotionDetailId = promotionDetailId;
            this.promotionId = promotionId;
            this.gameId = gameId;
            this.gameprofileId = gameprofileId;
            this.absoluteCredits = absoluteCredits;
            this.absoluteVIPCredits = absoluteVIPCredits;
            this.discountOnCredits = discountOnCredits;
            this.discountOnVIPCredits = discountOnVIPCredits;
            this.bonusAllowed = bonusAllowed;
            this.courtesyAllowed = courtesyAllowed;
            this.timeAllowed = timeAllowed;
            this.ticketAllowed = ticketAllowed;
            this.internetKey = internetKey;
            this.productId = productId;
            this.categoryId = categoryId;
            this.themeId = themeId;
            this.discountAmount = discountAmount; 
            this.visualizationThemeId = visualizationThemeId;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public PromotionDetailDTO(int promotionDetailId, int promotionId,int gameId,int gameprofileId,decimal? absoluteCredits,
                                   decimal? absoluteVIPCredits,decimal? discountOnCredits, decimal? discountOnVIPCredits,
                                   char? bonusAllowed,char? courtesyAllowed,char? timeAllowed,char? ticketAllowed,DateTime lastUpdatedDate,
                                   string lastUpdatedBy, int? internetKey, string guid, int siteId, bool synchStatus,int productId,
                                   int categoryId,int masterEntityId,int themeId,decimal? discountAmount,int visualizationThemeId,
                                   string createdBy,DateTime creationDate,bool isActive) 
            : this(promotionDetailId, promotionId, gameId, gameprofileId, absoluteCredits, absoluteVIPCredits,
                              discountOnCredits, discountOnVIPCredits, bonusAllowed, courtesyAllowed, timeAllowed, ticketAllowed,
                              internetKey, productId, categoryId, themeId, discountAmount, visualizationThemeId, isActive)
        {

            log.LogMethodEntry(promotionDetailId, promotionId, gameId, gameprofileId, absoluteCredits,absoluteVIPCredits, 
                              discountOnCredits, discountOnVIPCredits,bonusAllowed, courtesyAllowed, timeAllowed, ticketAllowed,
                              lastUpdatedDate,lastUpdatedBy, internetKey, guid, siteId, synchStatus, productId,categoryId, 
                              masterEntityId, themeId, discountAmount, visualizationThemeId, createdBy, creationDate);
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the PromotionDetail Id field
        /// </summary>
        public int PromotionDetailId
        {
            get { return promotionDetailId; }
            set { promotionDetailId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PromotionId field
        /// </summary>
        public int PromotionId
        {
            get { return promotionId; }
            set { promotionId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the GameId field
        /// </summary>
        public int GameId
        {
            get { return gameId; }
            set { gameId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the GameprofileId field
        /// </summary>
        public int GameprofileId
        {
            get { return gameprofileId; }
            set { gameprofileId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the AbsoluteCredits field
        /// </summary>
        public decimal? AbsoluteCredits
        {
            get { return absoluteCredits; }
            set { absoluteCredits = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the AbsoluteVIPCredits field
        /// </summary>
        public decimal? AbsoluteVIPCredits
        {
            get { return absoluteVIPCredits; }
            set { absoluteVIPCredits = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the DiscountOnCredits field
        /// </summary>
        public decimal? DiscountOnCredits
        {
            get { return discountOnCredits; }
            set { discountOnCredits = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the DiscountOnVIPCredits field
        /// </summary>
        public decimal? DiscountOnVIPCredits
        {
            get { return discountOnVIPCredits; }
            set { discountOnVIPCredits = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the BonusAllowed field
        /// </summary>
        public char? BonusAllowed
        {
            get { return bonusAllowed; }
            set { bonusAllowed = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CourtesyAllowed field
        /// </summary>
        public char? CourtesyAllowed
        {
            get { return courtesyAllowed; }
            set { courtesyAllowed = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TimeAllowed field
        /// </summary>
        public char? TimeAllowed
        {
            get { return timeAllowed; }
            set { timeAllowed = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TicketAllowed field
        /// </summary>
        public char? TicketAllowed
        {
            get { return ticketAllowed; }
            set { ticketAllowed = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the InternetKey field
        /// </summary>
        public int? InternetKey
        {
            get { return internetKey; }
            set { internetKey = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId
        {
            get { return productId; }
            set { productId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CategoryId field
        /// </summary>
        public int CategoryId
        {
            get { return categoryId ; }
            set { categoryId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the DiscountAmount field
        /// </summary>
        public decimal? DiscountAmount
        {
            get { return discountAmount; }
            set { discountAmount = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ThemeId field
        /// </summary>
        public int ThemeId
        {
            get { return themeId; }
            set { themeId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the VisualizationThemeId field
        /// </summary>
        public int VisualizationThemeId
        {
            get { return visualizationThemeId; }
            set { visualizationThemeId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || promotionDetailId < 0;
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
            IsChanged = false;
            log.LogMethodExit();
        }

    }
}
