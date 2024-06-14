
/********************************************************************************************
 * Project Name - POSPrintDisplayGroup
 * Description  - Data object of the POSPrintDisplayGroupDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        14-Nov-2016    Amaresh          Created 
 *2.90        22-Jul-2020    Girish            Modifed :  3 tier changes 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.DisplayGroup
{
    /// <summary>
    /// class of POSPrintDisplayGroupDTO
    /// </summary>
    public class POSPrintDisplayGroupDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByPosPrintDisplayGroupParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByPosPrintDisplayGroupParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,

            /// <summary>
            /// Search by POS_PRINTER_ID field
            /// </summary>
            POS_PRINTER_ID,

            /// <summary>
            /// Search by DISPLAYGROUP_ID field
            /// </summary>
            DISPLAYGROUP_ID,

            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int id;
        private int pOSPrinterId;
        private int displayGroupId;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        /// <summary>
        /// Default constructor
        /// </summary>
        public POSPrintDisplayGroupDTO()
        {
            log.LogMethodEntry();
            id = -1;
            displayGroupId = -1;
            pOSPrinterId = -1;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public POSPrintDisplayGroupDTO(int id, int pOSPrinterId, int displayGroupId)
            : this()
        {
            log.LogMethodEntry();
            this.id = id;
            this.pOSPrinterId = pOSPrinterId;
            this.displayGroupId = displayGroupId;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public POSPrintDisplayGroupDTO(int id, int pOSPrinterId, int displayGroupId, string guid,
                                               int siteId, bool synchStatus, int masterEntityId, DateTime creationDate,
                                               string createdBy, DateTime lastUpdateDate, string lastUpdatedBy)
            : this(id, pOSPrinterId, displayGroupId)
        {
            log.LogMethodEntry();
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the POSPrinterId field
        /// </summary>
        [DisplayName("POSPrinterId")]
        public int POSPrinterId { get { return pOSPrinterId; } set { pOSPrinterId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DisplayGroupId field
        /// </summary>
        [DisplayName("DisplayGroupId")]
        public int DisplayGroupId { get { return displayGroupId; } set { displayGroupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

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
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
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
        /// Get/Set method of the CreatedBy field
        /// </summary>
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
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate
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
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
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
