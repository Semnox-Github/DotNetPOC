using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// GameProfileParams Class
    /// </summary>
    public class GameParams
    {
        int gameId;
        int gameProfileId;
        String gameName;

        /// <summary>
        /// Get/Set method of the GameId field
        /// </summary>
        public int GameId { get { return gameId; } set { gameId = value; } }

        /// <summary>
        /// Get/Set method of the GameName field
        /// </summary>
        public String GameName { get { return gameName; } set { gameName = value; } }

        /// <summary>
        /// Get/Set method of the GameProfileId field
        /// </summary>
        public int GameProfileId { get { return gameProfileId; } set { gameProfileId = value; } }


    }

    
}
