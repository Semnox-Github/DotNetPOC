/********************************************************************************************
 * Project Name - ReportScheduleSitesDTO                                                                     
 * Description  - DTO for ReportScheduleSites Object
 *
 **************
 **Version Log
  *Version     Date          Modified By           Remarks          
 *********************************************************************************************
*2.110         04-Jan-2021   Laster Menezes        Created
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// ReportScheduleSitesDTO object
    /// </summary>
    public class ReportScheduleSitesDTO
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
            /// Search by SCHEDULE ID field
            /// </summary>
            SCHEDULE_ID,

            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int reportScheduleSitesId;
        private int scheduleId;
        private int reportScheduleSitesOrgId;
        private int reportScheduleSitesSiteId;        
        private string guid;
        private int siteId;
        private bool synchStatus;        
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportScheduleSitesDTO()
        {
            log.LogMethodEntry();
            reportScheduleSitesId = -1;
            scheduleId = -1;
            reportScheduleSitesOrgId = -1;
            reportScheduleSitesSiteId = -1;           
            siteId = -1;
            synchStatus = false;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public ReportScheduleSitesDTO(int reportScheduleSitesId, int scheduleId, int reportScheduleSitesOrgId, int reportScheduleSitesSiteId)
            : this()
        {
            log.LogMethodEntry(reportScheduleSitesId, scheduleId, reportScheduleSitesOrgId, reportScheduleSitesSiteId);
            this.reportScheduleSitesId = reportScheduleSitesId;
            this.scheduleId = scheduleId;
            this.reportScheduleSitesOrgId = reportScheduleSitesOrgId;
            this.reportScheduleSitesSiteId = reportScheduleSitesSiteId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ReportScheduleSitesDTO(int reportScheduleSitesId, int scheduleId, int reportScheduleSitesOrgId, int reportScheduleSitesSiteId, int siteId, string guid, 
                                      bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            : this(reportScheduleSitesId, scheduleId, reportScheduleSitesOrgId, reportScheduleSitesSiteId)
        {
            log.LogMethodEntry(reportScheduleSitesId, scheduleId, reportScheduleSitesOrgId, reportScheduleSitesSiteId, siteId, guid,
                                synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
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
        /// Get/Set method of the ReportScheduleSitesId field
        /// </summary>
        [DisplayName("ReportScheduleSitesId")]
        public int ReportScheduleSitesId { get { return reportScheduleSitesId; } set { reportScheduleSitesId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleId field
        /// </summary>
        [DisplayName("ScheduleId")]
        public int ScheduleId { get { return scheduleId; } set { scheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReportScheduleSitesOrgId field
        /// </summary>
        [DisplayName("ReportScheduleSitesOrgId")]
        public int ReportScheduleSitesOrgId { get { return reportScheduleSitesOrgId; } set { reportScheduleSitesOrgId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReportScheduleSitesSiteId field
        /// </summary>
        [DisplayName("ReportScheduleSitesSiteId")]
        public int ReportScheduleSitesSiteId { get { return reportScheduleSitesSiteId; } set { reportScheduleSitesSiteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || reportScheduleSitesId < 0;
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
