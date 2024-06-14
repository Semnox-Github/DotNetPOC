/********************************************************************************************
 * Project Name - Logger
 * Description  - Data object of MonitorApplication
 *
 **************
 ** Version Log
  **************
  * Version     Date Modified   By              Remarks
 *********************************************************************************************
 *2.70        29-May-2019       Divya          Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// This is the MonitorApplicationDTO data object class. This acts as data holder for the MonitorApplication business objects
    /// </summary>
    public class MonitorApplicationDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByMonitorApplicationParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByMonitorApplicationParameters
        {
            /// <summary>
            /// Search by Application Id field
            /// </summary>
            APPLICATION_ID,
            /// <summary>
            /// Search by Application Name field
            /// </summary>
            APPLICATION_NAME,
            /// <summary>
            /// Search by AppExeName field
            /// </summary>
            APP_EXE_NAME,
            /// <summary>
            /// Search by Site Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int applicationId;
        private string applicationName;
        private string appExeName;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor of MonitorApplicationDTO
        /// </summary>
        public MonitorApplicationDTO()
        {
            log.LogMethodEntry();
            applicationId = -1;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of MonitorApplicationDTO for required data fields
        /// </summary>
        public MonitorApplicationDTO(int applicationId, string applicationName, string appExeName)
            : this()
        {
            log.LogMethodEntry(applicationId, applicationName, appExeName);

            this.applicationId = applicationId;
            this.applicationName = applicationName;
            this.appExeName = appExeName;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor of MonitorApplicationDTO with All data fields
        /// </summary>
        public MonitorApplicationDTO(int applicationId, string applicationName, string appExeName, int siteId,
                                    string guid, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate,
                                    string lastUpdatedBy, DateTime lastUpdateDate)
            : this(applicationId, applicationName, appExeName)
        {
            log.LogMethodEntry(applicationId, applicationName, appExeName, siteId,
                                guid, synchStatus, masterEntityId, createdBy, creationDate,
                                lastUpdatedBy, lastUpdateDate);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ApplicationId field
        /// </summary>
        public int ApplicationId { get { return applicationId; } set { applicationId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ApplicationName field
        /// </summary>
        public string ApplicationName { get { return applicationName; } set { applicationName = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AppExeName field
        /// </summary>
        public string AppExeName { get { return appExeName; } set { appExeName = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

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
                    return notifyingObjectIsChanged || applicationId < 0;
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
