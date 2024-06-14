/********************************************************************************************
 * Project Name - AlohaTenderIdMaps
 * Description  - Data object of the AlohaTenderIdMapsDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        15-May-2017    Amaresh          Created 
 *2.70.2        24-Jul-2019    Deeksha          Modifications as per 3 tier standard.
 ********************************************************************************************/
using System;
using System.ComponentModel;
using Semnox.Parafait.logging;

namespace Semnox.Parafait.ThirdParty
{
    /// <summary>
    /// This is the AlohaTenderIdMaps data object class. This acts as data holder for the AlohaTenderIdMaps business object
    /// </summary>
    public class AlohaTenderIdMapsDTO
    {
        /// <summary>
        /// SearchByAlohaTenderIdMapsParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByAlohaTenderIdMapsParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID ,

            /// <summary>
            /// Search by ALOHA MAP ID field
            /// </summary>
            ALOHA_MAP_ID ,

            /// <summary>
            /// Search by CARD TYPE ID field
            /// </summary>
            CARD_TYPE_ID ,

            /// <summary>
            /// Search by TENDER ID field
            /// </summary>
            TENDER_ID ,

            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE ,

            /// <summary>
            /// Search by NAME field
            /// </summary>
            NAME ,

            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,

            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        private int alohaMapId;
        private string name;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private DateTime lastUpdatedDate;
        private string lastUpdatedUser;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        /// <summary>
        /// Default Contructor
        /// </summary>
        public AlohaTenderIdMapsDTO()
        {
            log.LogMethodEntry();
            alohaMapId = -1;
            isActive = true;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit() ;
        }

        /// <summary>
        /// Parameterized Constructor with required data fields
        /// </summary>    
        public AlohaTenderIdMapsDTO(int alohaMapId, string name, bool isActive)
            :this()
        {
            log.LogMethodEntry(alohaMapId, name, isActive);
            this.alohaMapId = alohaMapId;
            this.name = name;
            this.isActive = isActive;            
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>    
        public AlohaTenderIdMapsDTO(int alohaMapId, string name,bool isActive, string createdBy, DateTime creationDate, DateTime lastUpdatedDate, string lastUpdatedUser,
                                    string guid, int siteId, bool synchStatus, int masterEntityId)
            :this(alohaMapId, name, isActive)
        {
            log.LogMethodEntry( alohaMapId,  name,  isActive,  createdBy,  creationDate,  lastUpdatedDate,  lastUpdatedUser,
                                     guid,  siteId,  synchStatus,  masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the AlohaMapId field
        /// </summary>
        [DisplayName("AlohaMapId")]
        [ReadOnly(true)]
        public int AlohaMapId { get { return alohaMapId; } set { alohaMapId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("LastUpdatedUser")]
        public string LastUpdatedUser { get { return lastUpdatedUser; } set { lastUpdatedUser = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        ///  Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || alohaMapId < 0;
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
