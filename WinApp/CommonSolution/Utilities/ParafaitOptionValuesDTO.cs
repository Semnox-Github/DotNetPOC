/********************************************************************************************
 * Project Name - ParafaitOptionValuesDTO                                                                        
 * Description  - DTO for the ParafaitOptionValues tables.
 *
 **************
 **Version Log
  *Version     Date             Modified  By                  Remarks          
 *********************************************************************************************
 *2.40.1       25-Jan-2019    Flavia Jyothi Dsouza            Created new DTO class 
 *2.70.0       4 -Jul-2019    Girish Kundar                   Modified : Added new constructor with required Parameters 
 *                                                                       and making all data field as private.
 ********************************************************************************************/
using System;

namespace Semnox.Core.Utilities
{
    public class ParafaitOptionValuesDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// <summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  OptionValueId
            /// <summary>
            OPTION_VALUE_ID,
            /// <summary>
            /// Search by OptionId 
            /// </summary>
            OPTION_ID,
            /// <summary>
            /// Search by Option value
            /// </summary>
            OPTION_VALUE,
            /// <summary>
            /// Search by Active flag 
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by Site Id
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search By User Id 
            /// </summary>
            USER_ID,
            /// <summary>
            /// Search By POS machine Id
            /// </summary>
            POSMACHINEID,
            /// <summary>
            /// Search By master entity id 
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by OptionId List
            /// </summary>
            OPTION_ID_LIST

        }

        private int optionValueId;
        private int optionId;
        private string optionValue;
        private bool isActive;
        private int posMachineId;
        private int userId;
        private int masterEntityId;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private String createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// <summary>
        public ParafaitOptionValuesDTO()
        {
            log.LogMethodEntry();
            optionValueId = -1;
            optionId = -1;
            masterEntityId = -1;
            posMachineId = -1;
            userId = -1;
            siteId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// <summary>
        public ParafaitOptionValuesDTO(int optionValueId, int optionId, string optionValue, bool isActive,int posMachineId, int userId)
            :this()
        {
            log.LogMethodEntry(optionValueId, optionId, optionValue, isActive, posMachineId,  userId);
            this.optionValueId = optionValueId;
            this.optionId = optionId;
            this.optionValue = optionValue;
            this.isActive = isActive;
            this.posMachineId = posMachineId;
            this.userId = userId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// <summary>
        public ParafaitOptionValuesDTO(int optionValueId, int optionId, string optionValue, bool isActive,
                                       int posMachineId, int userId, int masterEntityId, string guid, int siteId,
                                       bool synchStatus, string lastUpdatedBy, DateTime lastUpdatedDate, String createdBy,
                                       DateTime creationDate)
            :this(optionValueId, optionId, optionValue, isActive, posMachineId, userId)
        {
            log.LogMethodEntry(optionValueId, optionId, optionValue, isActive, posMachineId,  userId,  masterEntityId,
                              guid,siteId,synchStatus,lastUpdatedBy, lastUpdatedDate,createdBy,creationDate);
            this.masterEntityId = masterEntityId;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            log.LogMethodExit();

        }
        /// <summary>
        /// Get/Set for OptionValueId
        /// </summary>
        public int OptionValueId
        {
            get
            {
                return optionValueId;
            }

            set
            {
                this.IsChanged = true;
                optionValueId = value;
            }
        }

        /// <summary>
        /// Get/Set for OptionId
        /// </summary>
        public int OptionId
        {
            get
            {
                return optionId;
            }

            set
            {
                this.IsChanged = true;
                optionId = value;
            }
        }

        /// <summary>
        /// Get/Set for  OptionValue
        /// </summary>
        public string OptionValue
        {
            get
            {
                return optionValue;
            }

            set
            {
                this.IsChanged = true;
                optionValue = value;
            }
        }

        /// <summary>
        // Get/Set for  IsActive  
        /// <summary>
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
        ///Get/Set for  PosMachineId
        public int PosMachineId
        {
            get
            {
                return posMachineId;
            }

            set
            {
                this.IsChanged = true;
                posMachineId = value;
            }
        }

        /// <summary>
        //Get/Set for  UserId
        /// <summary>
        public int UserId
        {
            get
            {
                return userId;
            }
            set
            {
                this.IsChanged = true;
                userId = value;
            }
        }

        /// <summary>
        /// Get for  Guid 
        /// </summary>
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
        ///  Get/Set for SiteId 
        /// </summary>
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                this.IsChanged = true;
                siteId = value;
            }
        }

        /// <summary>
        /// Get/Set for SynchStatus 
        /// <summary>

        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                synchStatus = value;
            }
        }

        /// <summary>
        /// Get/Set for  LastUpdatedBy 
        /// <summary>
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
        /// Get/Set for  LastUpdatedDate 
        /// <summary>
        public DateTime LastUpdatedDate
        {
            get
            {
                return lastUpdatedDate;
            }
            set
            {
                lastUpdatedDate = value;
            }
        }

        /// <summary>
        /// Get/Set for  MasterEntityId
        /// <summary>
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
        ///Get/Set for CreatedBy
        /// <summary>
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
        ///Get/Set for CreationDate 
        /// <summary>
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
        /// Allows to accept the changes to the value 
        /// <summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set  for IsChanged 
        /// <summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || optionValueId < 0;
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
    }
}
