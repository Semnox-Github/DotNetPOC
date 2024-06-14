using System;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// GameProfileParams Class
    /// </summary>
    public class GameProfileParams
    {
        private int gameProfileId;
        private String gameProfileName;

        /// <summary>
        /// Get/Set method of the GameProfileId field
        /// </summary>
        public int GameProfileId { get { return gameProfileId; } set { gameProfileId = value; } }

        /// <summary>
        /// Get/Set method of the GameProfileName field
        /// </summary>
        public String GameProfileName { get { return gameProfileName; } set { gameProfileName = value; } }
    }

    
}
