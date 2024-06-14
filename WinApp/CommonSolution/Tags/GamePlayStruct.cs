/********************************************************************************************
 * Project Name - CardCoreDTO Programs 
 * Description  - Data object of the GamePlayStruct
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00       01-Feb-2016   Rakshith           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Tags
{
    /// <summary> 
    /// GamePlayStruct Class
    /// </summary>
    public class GamePlayStruct
    {
        private int gamePlayId;
        private string gamePlayDate;
        private string gameName;
        private string gamePlayCredits;
        private string gamePlayBonus;
        private string gamePlayCourtesy;
        private string gamePlayTickets;
        private string gameCardGames;
        private string gameTicketMode;
        private string gameTime;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public GamePlayStruct()
        {
        }

        /// <summary>
        /// Parameterized Contructor
        /// </summary>
        public GamePlayStruct(int gamePlayId, string gamePlayDate, string gameName, string gamePlayCredits, string gamePlayBonus, string gamePlayCourtesy, string gamePlayTickets)
        {
            this.gamePlayId = gamePlayId;
            this.gamePlayDate = gamePlayDate;
            this.gameName = gameName;
            this.gamePlayCredits = gamePlayCredits;
            this.gamePlayBonus = gamePlayBonus;
            this.gamePlayCourtesy = gamePlayCourtesy;
            this.gamePlayTickets = gamePlayTickets;
        }

        /// <summary>
        /// Parameterized Contructor
        /// </summary>
        public GamePlayStruct(int gamePlayIdPassed, string gamePlayDatePassed, string gameNamePassed, string gamePlayCreditsPassed, string gamePlayBonusPassed, string gamePlayCourtesyPassed, string gamePlayTicketsPassed, string gameCardGamesPassed, string gameTicketModePassed, string gameTimePassed)
        {
            gamePlayId = gamePlayIdPassed;
            gamePlayDate = gamePlayDatePassed;
            gameName = gameNamePassed;
            gamePlayCredits = gamePlayCreditsPassed;
            gamePlayBonus = gamePlayBonusPassed;
            gamePlayCourtesy = gamePlayCourtesyPassed;
            gamePlayTickets = gamePlayTicketsPassed;
            gameCardGames = gameCardGamesPassed;
            gameTicketMode = gameTicketModePassed;
            gameTime = gameTimePassed;
        }

        /// <summary>
        /// Get/Set method of the GamePlayId field
        /// </summary>
        public int GamePlayId    { get { return gamePlayId; } set { gamePlayId = value; } }

        /// <summary>
        /// Get/Set method of the GamePlayDate field
        /// </summary>
        public string GamePlayDate { get { return gamePlayDate; } set { gamePlayDate = value; } }

        /// <summary>
        /// Get/Set method of the GameName field
        /// </summary>
        public string GameName { get { return gameName; } set { gameName = value; } }

        /// <summary>
        /// Get/Set method of the GamePlayCredits field
        /// </summary>
        public string GamePlayCredits { get { return gamePlayCredits; } set { gamePlayCredits = value; } }

        /// <summary>
        /// Get/Set method of the GamePlayBonus field
        /// </summary>
        public string GamePlayBonus { get { return gamePlayBonus; } set { gamePlayBonus = value; } }

        /// <summary>
        /// Get/Set method of the GamePlayCourtesy field
        /// </summary>
        public string GamePlayCourtesy  { get { return gamePlayCourtesy; } set { gamePlayCourtesy = value; } }

        /// <summary>
        /// Get/Set method of the GamePlayTickets field
        /// </summary>
        public string GamePlayTickets { get { return gamePlayTickets; } set { gamePlayTickets = value; } }

        /// <summary>
        /// Get/Set method of the GameCardGames field
        /// </summary>
        public string GameCardGames { get { return gameCardGames; } set { gameCardGames = value; } }

        /// <summary>
        /// Get/Set method of the GameTicketMode field
        /// </summary>
        public string GameTicketMode  {  get { return gameTicketMode; }  set { gameTicketMode = value; }  }

        /// <summary>
        /// Get/Set method of the GameTime field
        /// </summary>
        public string GameTime  { get { return gameTime; } set { gameTime = value; } }

       
    }
}
