using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Reader sotware version details Class
    /// </summary>
    public class SoftwareVersionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByGameParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByGameParameters
        {
            /// <summary>
            /// Search by machine id field
            /// </summary>
            MACHINE_ID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID
        }

        private int id; //Data fields made as private on 12-Aug-2019 by Deeksha
        private string versionNo;
        private string fileName;
        private string hash;
        private string isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default constructor
        /// </summary>
        public SoftwareVersionDTO()
        {
            log.LogMethodEntry();
            id = -1;
            isActive = "N";
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public SoftwareVersionDTO(int id, string versionNo, string fileName, 
                                  string hash, string isActive, string createdBy, DateTime creationDate,
                                  string lastUpdatedBy, DateTime lastUpdatedDate, int siteId, int masterEntityId,
                                  bool synchStatus, string guid)
        {
            log.LogMethodEntry( id,  versionNo,  fileName, hash,  isActive,  createdBy,  creationDate,
                                   lastUpdatedBy,  lastUpdatedDate,  siteId,  masterEntityId, synchStatus,  guid);
            this.id = id;
            this.versionNo = versionNo;
            this.fileName = fileName;
            this.hash = hash;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
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
        /// Get/Set method of the VersionNo field
        /// </summary>
        [DisplayName("Version No")]
        public string VersionNo
        {
            get
            {
                return versionNo;
            }

            set
            {
                this.IsChanged = true;
                versionNo = value;
            }
        }

        /// <summary>
        /// Get/Set method of the FileName field
        /// </summary>
        [DisplayName("FileName")]
        public string FileName
        {
            get
            {
                return fileName;
            }

            set
            {
                this.IsChanged = true;
                fileName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Hash field
        /// </summary>
        [DisplayName("Hash")]
        public string Hash
        {
            get
            {
                return hash;
            }

            set
            {
                this.IsChanged = true;
                hash = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active Flag")]
        public string IsActive
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
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdatedDate
        {
            get
            {
                return lastUpdatedDate;
            }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
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
        /// Get/Set method of the SynchStatus field
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
        /// Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
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
        /// Returns a string that represents the current SoftwareVersionDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------SoftwareVersionDTO-----------------------------\n");
            returnValue.Append(" Id : " + Id);
            returnValue.Append(" VersionNo : " + VersionNo);
            returnValue.Append(" IsActive : " + IsActive);
            returnValue.Append(" FileName : " + FileName);
            returnValue.Append(" Hash : " + Hash);
            returnValue.Append("\n-------------------------------------------------------------\n");
            string returnValues = returnValue.ToString();
            log.LogMethodExit(returnValues);
            return returnValues;

        }
    }
}
