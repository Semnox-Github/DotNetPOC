/********************************************************************************************
 * Project Name - CountryDTO
 * Description  - Data object of CountryDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By      Remarks          
 *********************************************************************************************
 *2.70.2        25-Jul-2019     Dakshakh raj     Modified : Added Parameterized costrustor,
 *                                                        IsActive field.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    public class CountryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by COUNTRY_ID field
            /// </summary>
            COUNTRY_ID,

            /// <summary>
            /// Search by COUNTRY_NAME field
            /// </summary>
            COUNTRY_NAME,

            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            ISACTIVE

        }

        int countryId;
        string countryName;
        string countryCode;
        string guid;
        int siteId;
        bool synchStatus;
        int masterEntityId;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdateDate;
        bool isActive;
        List<StateDTO> stateList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CountryDTO()
        {
            log.LogMethodEntry();
            countryId = -1;
            countryName = "";
            siteId = -1;
            synchStatus = false;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with required fields
        /// </summary>
        public CountryDTO(int countryId, string countryName, string countryCode)
            : this()
        {
            log.LogMethodEntry(countryId, countryName, countryCode);

            this.countryId = countryId;
            this.countryName = countryName;
            this.countryCode = countryCode;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with parameters
        /// </summary>
        public CountryDTO(int countryId, string countryName, string guid, int siteId, bool synchStatus, int masterEntityId,
                          string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, bool isActive, string countryCode)
            :this(countryId, countryName, countryCode)
        {
            log.LogMethodEntry(guid, siteId, synchStatus, masterEntityId, createdBy,
                               creationDate, lastUpdatedBy, lastUpdateDate, countryCode);

            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CountryId field
        /// </summary>
        [DisplayName("CountryId")]
        public int CountryId { get { return countryId; } set { countryId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CountryName field
        /// </summary>
        [DisplayName("CountryName")]
        public string CountryName { get { return countryName; } set { countryName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CountryCode field
        /// </summary>
        [DisplayName("CountryCode")]
        public string CountryCode { get { return countryCode; } set { countryCode = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; } }

        /// <summary>
        /// Get/Set method of the StateList field
        /// </summary>
        [DisplayName("StateList")]
        public List<StateDTO> StateList { get { return stateList; } set { stateList = value; } }
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
                    return notifyingObjectIsChanged || countryId < 0;
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
