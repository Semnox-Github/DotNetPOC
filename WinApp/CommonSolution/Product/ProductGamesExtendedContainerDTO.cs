/********************************************************************************************
 * Project Name - Product Games Extended Container DTO
 * Description  - Data object of Product Games Extended Object
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.150.0     07-Mar-2022   Prajwal S               Created 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the Product Games Extended data object class. This acts as data holder for the Product Games Extended business object
    /// </summary>
    public class ProductGamesExtendedContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int id;
        int productGameId;
        int gameId;
        int gameProfileId;
        bool exclude;
        string guid;
        int? playLimitPerGame;

        /// <summary>
        /// Default constructor of ProductGamesExtendedContainerDTO Class
        /// </summary>
        public ProductGamesExtendedContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields of ProductGamesExtendedContainerDTO class
        /// </summary>
        public ProductGamesExtendedContainerDTO(int id, int productGameId, int gameId, int gameProfileId, bool exclude, string guid, int? playLimitPerGame)
        {
            log.LogMethodEntry(id, productGameId, gameId, gameProfileId, exclude, guid, playLimitPerGame);
            this.id = id;
            this.productGameId = productGameId;
            this.gameId = gameId;
            this.gameProfileId = gameProfileId;
            this.exclude = exclude;
            this.guid = guid;
            this.playLimitPerGame = playLimitPerGame;
            log.LogMethodExit();
        }

        public ProductGamesExtendedContainerDTO(ProductGamesExtendedContainerDTO productGamesExtendedContainerDTO)
        : this(productGamesExtendedContainerDTO.id, productGamesExtendedContainerDTO.productGameId,
             productGamesExtendedContainerDTO.gameId, productGamesExtendedContainerDTO.gameProfileId,
             productGamesExtendedContainerDTO.exclude, productGamesExtendedContainerDTO.guid,
             productGamesExtendedContainerDTO.playLimitPerGame)
        {
            log.LogMethodEntry(productGamesExtendedContainerDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ProductGameId field
        /// </summary>
        public int ProductGameId
        {
            get
            {
                return productGameId;
            }
            set
            {
                productGameId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int GameId
        {
            get
            {
                return gameId;
            }
            set
            {
                gameId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the GameProfileId field
        /// </summary>
        public int GameProfileId
        {
            get
            {
                return gameProfileId;
            }
            set
            {
                gameProfileId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Exclude field
        /// </summary>
        public bool Exclude
        {
            get
            {
                return exclude;
            }
            set
            {
                exclude = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }
        }

        /// <summary>
        /// Get/Set method of the PlayLimitPerGame field
        /// </summary>
        public int? PlayLimitPerGame
        {
            get
            {
                return playLimitPerGame;
            }
            set
            {
                playLimitPerGame = value;
            }
        }
    }
}
