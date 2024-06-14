/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data object of the Promotion View
 * 
 **************
 **Version Log
 **************
 *Version      Date         Modified By       Remarks          
 *********************************************************************************************
 *2.150.2   02-Aug-2022    Abhishek           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Promotions
{
    ///<summary>
    /// This is a PromotionView data object class.
    /// </summary>
    public class PromotionViewDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ///<summary>
        ///SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by gameId field
            /// </summary>
            GAME_ID,
            ///<summary>
            /// Search by machineIdList field
            /// </summary>
            MACHINE_ID_LIST,
            ///<summary>
            /// Search by gameProfileId field
            /// </summary>
            GAME_PROFILE_ID,
            /// <summary>
            /// Search by membershipId field
            /// </summary>
            MEMBERSHIP_ID
        }

        private int gameId;
        private int gameProfileId;
        private int membershipId;
        private decimal? absoluteCredits;
        private decimal? discountOnCredits;
        private decimal? absoluteVipCredits;
        private decimal? discountOnVipCredits;
        private int promotionId;
        private int promotionDetailId;
        private string bonusAllowed;
        private string courtesyAllowed;
        private string timeAllowed;
        private string ticketAllowed;
        private int themeNumber;
        private int visualizationThemeNumber;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PromotionViewDTO()
        {
            log.LogMethodEntry();
            gameId = -1;
            gameProfileId = -1;
            absoluteCredits = -1;
            membershipId = -1;
            promotionId = -1;
            promotionDetailId = -1;
            courtesyAllowed = "N";
            timeAllowed = "N";
            bonusAllowed = "N";
            ticketAllowed = "N";
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor Required data fields
        /// </summary>

        public PromotionViewDTO(int gameId, int gameProfileId, decimal? absoluteCredits, decimal? discountOnCredits, decimal? absoluteVipCredits, decimal? discountOnVipCredits,
                                int promotionId, int promotionDetailId, string bonusAllowed, string courtesyAllowed, string timeAllowed, string ticketAllowed,
                                int themeNumber, int visualizationThemeNumber)
            : this()
        {
            log.LogMethodEntry(gameId, gameProfileId, absoluteCredits, discountOnCredits,  absoluteVipCredits, discountOnVipCredits, 
                               promotionId, promotionDetailId, bonusAllowed, courtesyAllowed, timeAllowed, ticketAllowed, themeNumber, visualizationThemeNumber);
            this.gameId = gameId;
            this.gameProfileId = gameProfileId;
            this.absoluteCredits = absoluteCredits;
            this.discountOnCredits = discountOnCredits;
            this.absoluteVipCredits = absoluteVipCredits;
            this.discountOnVipCredits = discountOnVipCredits;
            this.promotionId = promotionId;
            this.promotionDetailId = promotionDetailId;
            this.bonusAllowed = bonusAllowed;
            this.courtesyAllowed = courtesyAllowed;
            this.timeAllowed = timeAllowed;
            this.ticketAllowed = ticketAllowed;
            this.themeNumber = themeNumber;
            this.visualizationThemeNumber = visualizationThemeNumber;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the gameId field
        /// </summary>
        public int GameId { get { return gameId; } set { gameId = value; } }

        /// <summary>
        /// Get/Set method of the gameProfileId field
        /// </summary>
        public int GameProfileId { get { return gameProfileId; } set { gameProfileId = value; } }

        /// <summary>
        /// Get/Set method of the membershipId field
        /// </summary>
        public int MembershipId { get { return membershipId; } set { membershipId = value; } }

        /// <summary>
        /// Get/Set method of the absoluteCredits field
        /// </summary>
        public decimal? AbsoluteCredits { get { return absoluteCredits; } set { absoluteCredits = value; } }

        /// <summary>
        /// Get/Set method of the discountOnCredits field
        /// </summary>
        public decimal? DiscountOnCredits { get { return discountOnCredits; } set { discountOnCredits = value; } }

        /// <summary>
        /// Get/Set method of the absoluteVipCredits field
        /// </summary>
        public decimal? AbsoluteVipCredits { get { return absoluteVipCredits; } set { absoluteVipCredits = value; } }

        /// <summary>
        /// Get/Set method of the discountOnVipCredits field
        /// </summary>
        public decimal? DiscountOnVipCredits { get { return discountOnVipCredits; } set { discountOnVipCredits = value; } }

        /// <summary>
        /// Get/Set method of the promotionId field
        /// </summary>
        public int PromotionId { get { return promotionId; } set { promotionId = value; } }

        /// <summary>
        /// Get/Set method of the promotionDetailId field
        /// </summary>
        public int PromotionDetailId { get { return promotionDetailId; } set { promotionDetailId = value; } }

        /// <summary>
        /// Get/Set method of the bonusAllowed field
        /// </summary>
        public string BonusAllowed { get { return bonusAllowed; } set { bonusAllowed = value; } }

        /// <summary>
        /// Get/Set method of the courtesyAllowed field
        /// </summary>
        public string CourtesyAllowed { get { return courtesyAllowed; } set { courtesyAllowed = value; } }

        /// <summary>
        /// Get/Set method of the timeAllowed field
        /// </summary>
        public string TimeAllowed { get { return timeAllowed; } set { timeAllowed = value; } }

        /// <summary>
        /// Get/Set method of the ticketAllowed field
        /// </summary>
        public string TicketAllowed { get { return ticketAllowed; } set { ticketAllowed = value; } }

        /// <summary>
        /// Get/Set method of the ticketAllowed field
        /// </summary>
        public int ThemeNumber { get { return themeNumber; } set { themeNumber = value; } }

        /// <summary>
        /// Get/Set method of the visualizationThemeNumber field
        /// </summary>
        public int VisualizationThemeNumber { get { return visualizationThemeNumber; } set { visualizationThemeNumber = value; } }
    }
}
