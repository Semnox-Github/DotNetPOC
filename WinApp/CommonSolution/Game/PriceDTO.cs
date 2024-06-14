namespace Semnox.Parafait.Game
{
    /// <summary>
    /// PriceDTO
    /// </summary>
    public class PriceDTO
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int gameId;
        private double playCredits;
        private double vipPlayCredits;
        private string formattedPlayCredits;
        private string formattedVipPlayPlayCredits;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PriceDTO()
        {
            log.LogMethodEntry();
            gameId = -1;
            playCredits = 0;
            vipPlayCredits = 0;
            formattedPlayCredits = string.Empty;
            formattedVipPlayPlayCredits = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public PriceDTO(int gameId, double playCredits, double vipPlayCredits, string formattedPlayCredits, string formattedVipPlayPlayCredits)
        {
            log.LogMethodEntry(gameId, playCredits, vipPlayCredits);
            this.gameId = gameId;
            this.playCredits = playCredits;
            this.vipPlayCredits = vipPlayCredits;
            this.formattedPlayCredits = formattedPlayCredits;
            this.formattedVipPlayPlayCredits = formattedVipPlayPlayCredits;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method for MachineId
        /// </summary>
        public int GameId { get { return gameId; } set { gameId = value; } }
        /// <summary>
        /// Get/Set method for PlayCredits
        /// </summary>
        public double PlayCredits { get { return playCredits; } set { playCredits = value; } }
        /// <summary>
        /// Get/Set method for VipPlayCredits
        /// </summary>
        public double VipPlayCredits { get { return vipPlayCredits; } set { vipPlayCredits = value; } }

        /// <summary>
        /// Get/Set method for PlayCredits
        /// </summary>
        public string FormattedPlayCredits { get { return formattedPlayCredits; } set { formattedPlayCredits = value; } }
        /// <summary>
        /// Get/Set method for VipPlayCredits
        /// </summary>
        public string FormattedVipPlayPlayCredits { get { return formattedVipPlayPlayCredits; } set { formattedVipPlayPlayCredits = value; } }
    }
}
