/********************************************************************************************
 * Project Name - 
 * Description  - 
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By             Remarks          
 *********************************************************************************************
 **2.150.02   21-Mar-23    Yashodhara C H
 ********************************************************************************************/

using System;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the AccountCreditPlus Consumption summary data object class.
    /// </summary>
    public class AccountGameExtendedSummaryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountGameExtendedSummaryDTO()
        {
            log.LogMethodEntry();
            AccountGameExtendedId = -1;
            AccountGameId = -1;
            GameId = -1;
            GameProfileId = -1;
            PlayLimitPerGame = 0;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AccountGameExtendedSummaryDTO(int accountGameExtendedId, int accountGameId, int gameId, string gameName, int gameProfileId,
                         string gameProfileName, bool exclude, int playLimitPerGame)
            : this()
        {
            log.LogMethodEntry(accountGameExtendedId, accountGameId, gameId, gameName, gameProfileId, gameProfileName, exclude, playLimitPerGame);
            AccountGameExtendedId = accountGameExtendedId;
            AccountGameId = accountGameId;
            GameId = gameId;
            GameName = gameName;
            GameProfileId = gameProfileId;
            GameProfileName = gameProfileName;
            Exclude = exclude;
            PlayLimitPerGame = playLimitPerGame;
            log.LogMethodExit();
        }


        public AccountGameExtendedSummaryDTO(AccountGameExtendedSummaryDTO accountGameExtendedSummaryDTO)
          : this(accountGameExtendedSummaryDTO.AccountGameExtendedId,
                accountGameExtendedSummaryDTO.AccountGameId,
                 accountGameExtendedSummaryDTO.GameId,
                 accountGameExtendedSummaryDTO.GameName,
                 accountGameExtendedSummaryDTO.GameProfileId,
                 accountGameExtendedSummaryDTO.GameProfileName,
                 accountGameExtendedSummaryDTO.Exclude,
                 accountGameExtendedSummaryDTO.PlayLimitPerGame)
        {
            log.LogMethodEntry();

            log.LogMethodExit();
        }


        public int AccountGameExtendedId { get; set; }
        public int AccountGameId { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public int GameProfileId { get; set; }
        public string GameProfileName { get; set; }
        public bool Exclude { get; set; }
        public int PlayLimitPerGame { get; set; }
    }
}
