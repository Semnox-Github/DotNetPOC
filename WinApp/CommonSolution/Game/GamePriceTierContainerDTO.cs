/********************************************************************************************
* Project Name - Game
* Description  - DTO for Game Container Class.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     10-Dec-2020     Prajwal S             Created : Web Inventory UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    public class GamePriceTierContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int gamePriceTierId;
        private int gameId;
        private int gameProfileId;
        private string name;
        private string description;
        private int playCount;
        private decimal playCredits;
        private decimal vipPlayCredits;
        private int sortOrder;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GamePriceTierContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public GamePriceTierContainerDTO(int gamePriceTierId, int gameId, 
                                         int gameProfileId, string name, 
                                         string description, int playCount, 
                                         decimal playCredits, decimal vipPlayCredits, 
                                         int sortOrder)
            : this()
        {
            log.LogMethodEntry(gamePriceTierId, gameId, gameProfileId, name, description, playCount, playCredits, vipPlayCredits, sortOrder);
            this.gamePriceTierId = gamePriceTierId;
            this.gameId = gameId;
            this.gameProfileId = gameProfileId;
            this.name = name;
            this.description = description;
            this.playCount = playCount;
            this.playCredits = playCredits;
            this.vipPlayCredits = vipPlayCredits;
            this.sortOrder = sortOrder;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the tierId field
        /// </summary>
        public int GamePriceTierId { get { return gamePriceTierId; } set { gamePriceTierId = value; } }
        /// <summary>
        /// Get/Set method of the gameId field
        /// </summary>
        public int GameId { get { return gameId; } set { gameId = value; } }

        /// <summary>
        /// Get/Set method of the gameProfileId field
        /// </summary>
        public int GameProfileId { get { return gameProfileId; } set { gameProfileId = value; } }

        /// <summary>
        /// Get/Set method of the name field
        /// </summary>
        public string Name { get { return name; } set { name = value; } }
        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        public string Description { get { return description; } set { description = value; } }

        /// <summary>
        /// Get/Set method of the playCount field
        /// </summary>
        public int PlayCount { get { return playCount; } set { playCount = value; } }

        /// <summary>
        /// Get/Set method of the price field
        /// </summary>
        public decimal PlayCredits { get { return playCredits; } set { playCredits = value; } }

        /// <summary>
        /// Get/Set method of the price field
        /// </summary>
        public decimal VipPlayCredits { get { return vipPlayCredits; } set { vipPlayCredits = value; } }
        /// <summary>
        /// Get/Set method of the sortOrder field
        /// </summary>
        public int SortOrder { get { return sortOrder; } set { sortOrder = value; } }
    }
}
