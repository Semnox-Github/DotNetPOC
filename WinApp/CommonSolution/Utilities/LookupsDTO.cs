/********************************************************************************************
 * Project Name - Lookups
 * Description  - DTO of lookups
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        13-Mar-2016   Jagan Mohana   Created
 *2.60        09-Apr-2019   Mushahid Faizan  Modified : Added LogMethodEntry/Exit.
 *2.70        3- Jul- 2019  Girish Kundar  Modified : Added Constructor with required Parameter
 *                                                    and IsChangedRecurssive for DTOList.  
  ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Core.Utilities
{
    public class LookupsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by LOOKUP ID field
            /// </summary>
            LOOKUP_ID,
            /// <summary>
            /// Search by LOOKUP NAME field
            /// </summary>
            LOOKUP_NAME,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int lookupId;
        private string lookupName;
        private string isProtected;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool isActive;
        private List<LookupValuesDTO> lookupValuesDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public LookupsDTO()
        {
            log.LogMethodEntry();
            this.lookupId = -1;
            this.siteId = -1;
            this.masterEntityId = -1;
            this.isActive = true;
            this.lookupValuesDTOList = new List<LookupValuesDTO>();
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public LookupsDTO(int lookupId, string lookupName, string isProtected ,bool isActive)
            : this()
        {
            log.LogMethodEntry(lookupId, lookupName, IsProtected, isActive);
            this.lookupId = lookupId;
            this.lookupName = lookupName;
            this.isProtected = isProtected;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public LookupsDTO(int lookupId, string lookupName, string isProtected, string guid, bool synchStatus, int siteId,
                            int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,bool isActive)
            : this(lookupId, lookupName, isProtected, isActive)
        {
            log.LogMethodEntry(lookupId, lookupName, IsProtected, guid, synchStatus, siteId, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, isActive);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LookupId field
        /// </summary>
        [DisplayName("Lookup Id")]
        [ReadOnly(true)]
        public int LookupId { get { return lookupId; } set { lookupId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LookupName field
        /// </summary>
        [DisplayName("Lookup Name")]
        public string LookupName { get { return lookupName; } set { lookupName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Protected field
        /// </summary>
        [DisplayName("Protected")]
        public string IsProtected { get { return isProtected; } set { isProtected = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GUID field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SyncStatus field
        /// </summary>
        [DisplayName("SyncStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set methods for LookupValuesDTOList 
        /// </summary>
        public List<LookupValuesDTO> LookupValuesDTOList
        {
            get
            {
                return lookupValuesDTOList;
            }

            set
            {
                lookupValuesDTOList = value;
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
                    return notifyingObjectIsChanged || lookupId < 0;
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
        /// Returns whether the LookupDTO is changed or any of its  children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (lookupValuesDTOList != null &&
                    lookupValuesDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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