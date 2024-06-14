/********************************************************************************************
 * Project Name - MembershipExclusionRule  DTO Programs 
 * Description  - Data object of the MembershipExclusionRuleDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        23-Jan-2019   Ankitha C Kothwal   Created 
 *2.110.00    26-Nov-2020   Abhishek            Modified : Modified to 3 Tier Standard
 ********************************************************************************************/
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class MembershipExclusionRuleDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>        
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by  Product_Id field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by  gameId field
            /// </summary>
            GAME_ID,
            /// <summary>
            /// Search by  gameProfileIdfield
            /// </summary>
            GAME_PROFILE_ID,
            /// <summary>
            /// Search by  active field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by  DISALLOWED field
            /// </summary>
            DISALLOWED,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by  membership field
            /// </summary>
            MEMBERSHIP_ID,
            /// <summary>
            /// Search by LAST_UPDATED_BY field
            /// </summary>
            LAST_UPDATED_BY
        }

        private int id;
        private int productId;
        private int gameId;
        private int gameProfileId;
        private bool isActive;
        private bool disallowed;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId;
        private int membershipId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public MembershipExclusionRuleDTO()
        {
            log.LogMethodEntry();
            id = -1;
            gameId = -1;
            gameProfileId = -1;
            siteId = -1;
            isActive = true;
            productId = -1;
            membershipId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameters
        /// </summary>
        public MembershipExclusionRuleDTO(int id, int productId, int gameId, int gameProfileId, bool isActive, bool disallowed, int membershipId)
            : this()
        {
            log.LogMethodEntry(id, productId, gameId, gameProfileId, isActive, disallowed, membershipId);
            this.id = id;
            this.productId = productId;
            this.gameId = gameId;
            this.gameProfileId = gameProfileId;
            this.membershipId = membershipId;
            this.isActive = isActive;
            this.disallowed = disallowed;
            this.membershipId = membershipId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MembershipExclusionRuleDTO(int id, int productId, int gameId, int gameProfileId, bool isActive, bool disallowed, string lastUpdatedBy,
                                         DateTime lastUpdatedDate, string guid, bool synchStatus, int siteId, int masterEntityId, int membershipId,
                                         string createdBy, DateTime creationDate)
           : this(id, productId, gameId, gameProfileId, isActive, disallowed, membershipId)
        {
            log.LogMethodEntry(id, productId, gameId, gameProfileId, isActive, disallowed, lastUpdatedBy, lastUpdatedDate, guid, synchStatus, siteId,
                               masterEntityId, membershipId, createdBy, creationDate);
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>      
        public int Id { get { return id; } set { this.IsChanged = true; id = value; } }

        /// <summary>
        /// Get/Set method of the productId field
        /// </summary>      
        public int ProductId { get { return productId; } set { this.IsChanged = true; productId = value; } }

        /// <summary>
        /// Get/Set method of the GameId field
        /// </summary>        
        public int GameId { get { return gameId; } set { this.IsChanged = true; gameId = value; } }

        /// <summary>
        /// Get/Set method of the GameProfileId field
        /// </summary>       
        public int GameProfileId { get { return gameProfileId; } set { this.IsChanged = true; gameProfileId = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>        
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

        /// <summary>
        /// Get/Set method of the Disallowed field
        /// </summary>      
        public bool Disallowed { get { return disallowed; } set { this.IsChanged = true; disallowed = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdtedBy field
        /// </summary>     
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdtedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>      
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>    
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the MembershipId field
        /// </summary>  
        public int MembershipId { get { return membershipId; } set { this.IsChanged = true; membershipId = value; } }

        /// <summary>
        /// Get/Set method of the createdBy field
        /// </summary>       
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the craetion date field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
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
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
