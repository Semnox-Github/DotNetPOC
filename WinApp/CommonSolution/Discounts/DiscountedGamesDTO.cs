/********************************************************************************************
 * Project Name - DiscountedGames DTO
 * Description  - Data object of DiscountedGames
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        15-Jul-2017   Lakshminarayana          Created 
 *2.70        17-Mar-2019   Akshay Gulaganji         Modified isActive (string to bool) 
 *2.70.2        30-Jul-2019   Girish Kundar            Modified : Added constructor with required Parameter,Missing Who columns
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// This is the DiscountedGames data object class. This acts as data holder for the DiscountedGames business object
    /// </summary>
    public class DiscountedGamesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int id;
        private int discountId;
        private int gameId;
        private string discounted;
        private bool isActive;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscountedGamesDTO()
        {
            log.LogMethodEntry();
            id = -1;
            discountId = -1;
            gameId = -1;
            discounted = "Y";
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DiscountedGamesDTO(int id, int discountId, int gameId, string discounted, bool isActive)
            : this()
        {
            log.LogMethodEntry(id, discountId, gameId, discounted, isActive);
            this.id = id;
            this.discountId = discountId;
            this.gameId = gameId;
            this.discounted = discounted;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DiscountedGamesDTO(int id, int discountId, int gameId, string discounted, bool isActive, int siteId,
                                   int masterEntityId, bool synchStatus, string guid, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                  DateTime lastUpdateDate)
            : this(id, discountId, gameId, discounted, isActive)
        {
            log.LogMethodEntry(id, discountId, gameId, discounted, isActive, siteId,
                                   masterEntityId, synchStatus, guid, createdBy, creationDate, lastUpdatedBy,
                                   lastUpdateDate);
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public DiscountedGamesDTO(DiscountedGamesDTO discountedGamesDTO)
            : this(discountedGamesDTO.id,
                   discountedGamesDTO.discountId,
                   discountedGamesDTO.gameId,
                   discountedGamesDTO.discounted,
                   discountedGamesDTO.isActive,
                   discountedGamesDTO.siteId,
                   discountedGamesDTO.masterEntityId,
                   discountedGamesDTO.synchStatus,
                   discountedGamesDTO.guid,
                   discountedGamesDTO.createdBy,
                   discountedGamesDTO.creationDate,
                   discountedGamesDTO.lastUpdatedBy,
                   discountedGamesDTO.lastUpdateDate)
        {
            log.LogMethodEntry(discountedGamesDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                this.IsChanged = true;
                id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the discountId field
        /// </summary>
        [Browsable(false)]
        public int DiscountId
        {
            get
            {
                return discountId;
            }

            set
            {
                this.IsChanged = true;
                discountId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the GameId field
        /// </summary>
        [DisplayName("Game")]
        public int GameId
        {
            get
            {
                return gameId;
            }

            set
            {
                this.IsChanged = true;
                gameId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Discounted field
        /// </summary>
        [DisplayName("Discounted")]
        public string Discounted
        {
            get
            {
                return discounted;
            }

            set
            {
                this.IsChanged = true;
                discounted = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Is Active?")]
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }

        /// <summary>
        /// Get method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                siteId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }

            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }

        /// <summary>
        /// Get method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
        }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
            }
        }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Get method of the LastUpdateDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {
                lastUpdateDate = value;
            }
        }


        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
            }
        }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
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
                    return notifyingObjectIsChanged || id < 0;
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

        /// <summary>
        /// Returns a string that represents the current DiscountedGamesDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------DiscountedGamesDTO-----------------------------\n");
            returnValue.Append(" Id : " + Id);
            returnValue.Append(" DiscountId : " + DiscountId);
            returnValue.Append(" GameId : " + GameId);
            returnValue.Append(" Discounted : " + Discounted);
            returnValue.Append(" IsActive : " + IsActive);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit(returnValue);
            return returnValue.ToString();

        }
    }
}
