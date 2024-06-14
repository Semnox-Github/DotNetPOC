/***************************************************************************************************
 * Project Name - GamePlayDTO                                                                     
 * Description  - DTO for GamePlay Object
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 ****************************************************************************************************
 *2.150.2     12-Dec-2022   Mathew Ninan          Created: GamePlayBuildDTO for performing Gameplay
 *2.155.0     15-Jul-2023   Mathew Ninan          Added additional properties to support Gameplay.
 *                                                MultiGamePlayReference and GamePriceTierInfo.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.ServerCore
{
    /// <summary>
    /// GamePlayDTO Object
    /// </summary>
    public class GamePlayBuildDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        
        private string gameplayType;
        private GamePlayDTO gameplayDTO;
        private GameServerEnvironment.GameServerPlayDTO gameServerPlayDTO;
        private string requestURL;
        private string price;
        private string discountPercentage;
        private string recordId;
        private string cardGamePlay;
        private string counter;
        private int readwriteTimeout;
        private bool commitGamePlay;
        private Guid? multiGamePlayReference;
        private string gamePriceTierInfo;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GamePlayBuildDTO()
        {
            log.LogMethodEntry();
            gameplayType = string.Empty;
            gameplayDTO = new GamePlayDTO();
            gameServerPlayDTO = new GameServerEnvironment.GameServerPlayDTO();
            requestURL = string.Empty;
            readwriteTimeout = 20;
            commitGamePlay = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with  required parameters
        /// </summary>
        public GamePlayBuildDTO(string gameplayType, GamePlayDTO gameplayDTO, 
                                GameServerEnvironment.GameServerPlayDTO gameServerPlayDTO,
                                string price, string discountPercentage, string recordId, string cardGamePlay, 
                                string requestURL, int readwriteTimeout, bool shouldCommit)
            : this()
        {
            log.LogMethodEntry(gameplayType, gameplayDTO, gameServerPlayDTO, requestURL, readwriteTimeout, shouldCommit);

            this.gameplayType = gameplayType;
            this.gameplayDTO = gameplayDTO;
            this.gameServerPlayDTO = gameServerPlayDTO;
            this.price = price;
            this.discountPercentage = discountPercentage;
            this.recordId = recordId;
            this.cardGamePlay = cardGamePlay;
            //this.counter = counter;
            this.requestURL = requestURL;
            this.readwriteTimeout = readwriteTimeout;
            this.commitGamePlay = shouldCommit;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with  required parameters
        /// </summary>
        public GamePlayBuildDTO(string gameplayType, GamePlayDTO gameplayDTO,
                                GameServerEnvironment.GameServerPlayDTO gameServerPlayDTO,
                                string price, string discountPercentage, string recordId, string cardGamePlay,
                                string requestURL, int readwriteTimeout, bool shouldCommit, Guid? multiGamePlayReference,
                                string gamePriceTierInfo)
            : this(gameplayType, gameplayDTO, gameServerPlayDTO, price, discountPercentage, recordId, cardGamePlay, requestURL,
                  readwriteTimeout, shouldCommit)
        {
            log.LogMethodEntry(gameplayType, gameplayDTO, gameServerPlayDTO, requestURL, readwriteTimeout, shouldCommit);
            this.multiGamePlayReference = multiGamePlayReference;
            this.gamePriceTierInfo = gamePriceTierInfo;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the GameplayId field
        /// </summary>
        [DisplayName("GameplayType")]
        public string GameplayType { get { return gameplayType; } set { gameplayType = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the MachineId field
        /// </summary>
        [DisplayName("GamePlayDTO")]
        public GamePlayDTO GamePlayDTO { get { return gameplayDTO; } set { gameplayDTO = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MachineId field
        /// </summary>
        [DisplayName("GameServerPlayDTO")]
        public GameServerEnvironment.GameServerPlayDTO GameServerPlayDTO
        { get { return gameServerPlayDTO; }
            set { gameServerPlayDTO = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("RequestURL")]
        public string RequestURL { get { return requestURL; } set { requestURL = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        [DisplayName("ReadWriteTimeout")]
        public int ReadWriteTimeout { get { return readwriteTimeout; } set { readwriteTimeout = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Credits field
        /// </summary>
        [DisplayName("CommitGamePlay")]
        public bool CommitGamePlay { get { return commitGamePlay; } set { commitGamePlay = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>
        public string Price { get { return price; } set { price = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DiscountPercentage field
        /// </summary>
        public string DiscountPercentage { get { return discountPercentage; } set { requestURL = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RecordId field
        /// </summary>
        public string RecordId { get { return recordId; } set { recordId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardGamePlay field
        /// </summary>
        public string CardGamePlay { get { return cardGamePlay; } set { cardGamePlay = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Counter field
        /// </summary>
        public string Counter { get { return counter; } set { counter = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MultiGamePlayReference field
        /// </summary>
        [DisplayName("MultiGamePlayReference")]
        public Guid? MultiGamePlayReference { get { return multiGamePlayReference; } set { multiGamePlayReference = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GamePriceTierInfo field
        /// </summary>
        [DisplayName("GamePriceTierInfo")]
        public string GamePriceTierInfo { get { return gamePriceTierInfo; } set { gamePriceTierInfo = value; this.IsChanged = true; } }


        /// <summary>
        /// Returns whether the GamePlayDTO changed or any of its List  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (gameplayDTO != null &&
                   gameplayDTO.IsChanged)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged;
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
