
/********************************************************************************************
 * Project Name - ReportsScheduleEmail
 * Description  - Data object of the ReportsScheduleEmailDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *1.00        14-Apil-2017      Archana          Created 
 *2.70.2        11-Jul-2019     Dakshakh raj       Modified : Added Parameterized costrustor. Added createdBy,creationDate,
 *                                                          lastUpdatedBy,lastUpdateDate fields.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// class of ReportsScheduleEmailDTO 
    /// </summary>
    public class ReportsScheduleEmailDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByReportScheduleEmailIdParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByReportScheduleEmailIdParameters
        {
            /// <summary>
            /// Search by ReportScheduleEmailId field
            /// </summary>
            REPORT_SCHEDULE_EMAIL_ID ,

            /// <summary>
            /// Search by SCHEDULE ID field
            /// </summary>
            SCHEDULE_ID,

            /// <summary>
            /// Search by EMAIL ID field
            /// </summary>
            EMAIL_ID,

            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by MASTERENTITYID field
            /// </summary>
            MASTERENTITYID
        }

        private int reportScheduleEmailId;
        private int scheduleId;
        private string emailId;
        private string name;
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
        public ReportsScheduleEmailDTO()
        {
            log.LogMethodEntry();
            reportScheduleEmailId = -1;
            scheduleId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public ReportsScheduleEmailDTO(int reportScheduleEmailId, int scheduleId, string emailId, string name)
            : this()
        {
            log.LogMethodEntry(reportScheduleEmailId, scheduleId, emailId, name);
            this.reportScheduleEmailId = reportScheduleEmailId;
            this.scheduleId = scheduleId;
            this.emailId = emailId;
            this.name = name;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the data fields
        /// </summary>
        public ReportsScheduleEmailDTO(int reportScheduleEmailId, int scheduleId, string emailId, string name, string guid, int siteId, bool synchStatus, int masterEntityId,
                                       string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            :this(reportScheduleEmailId, scheduleId, emailId, name)
        {
            log.LogMethodEntry(createdBy,creationDate,lastUpdateDate,lastUpdatedBy,guid,siteId,masterEntityId,synchStatus);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ReportScheduleEmailId field
        /// </summary>
        [DisplayName("Report Schedule EmailId")]
        [ReadOnly(true)]
        public int ReportScheduleEmailId { get { return reportScheduleEmailId; } set { reportScheduleEmailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleId field
        /// </summary>
        [DisplayName("Schedule Id")]
        public int ScheduleId { get { return scheduleId; } set { scheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EmailId field
        /// </summary>
        [DisplayName("Email Id")]
        public string EmailId { get { return emailId; } set { emailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>        
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

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
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;} }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [Browsable(false)]
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
                    return notifyingObjectIsChanged || reportScheduleEmailId < 0;
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
