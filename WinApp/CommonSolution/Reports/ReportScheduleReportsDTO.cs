/****************************************************************************************************************
 * Project Name - Reports
 * Description  - Data object of Report schedule Report DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *****************************************************************************************************************
 *2.70.2       10-Jul-2019   Dakshakh raj    Modified : Added Parameterized costrustor. Added createdBy,creationDate,
 *                                                    lastUpdatedBy,lastUpdateDate fields
 *****************************************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    ///  This is the ReportScheduleReports data object class. This acts as data holder for the ReportsScheduleReports business object
    /// </summary> 
    public class ReportScheduleReportsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByReportScheduleReportsParameters
        {
            /// <summary>
            /// Search by REPORT SCHEDULE REPORT ID field
            /// </summary>
            REPORT_SCHEDULE_REPORT_ID,

            /// <summary>
            /// Search by SCHEDULE ID field
            /// </summary>
            SCHEDULE_ID,

            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by MASTERENTITYID field
            /// </summary>
            MASTERENTITYID,

            /// <summary>
            /// Search by REPORT ID field
            /// </summary>
            REPORT_ID
        }

        private int reportScheduleReportId;
        private int scheduleId;
        private int reportId;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private string outputFormat;
        private int masterEntityId;
        private DateTime lastSuccessfulRunTime;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportScheduleReportsDTO()
        {
            log.LogMethodEntry();
            reportScheduleReportId = -1;
            scheduleId = -1;
            reportId = -1;
            siteId = -1;
            masterEntityId = -1;
            outputFormat = "P";
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public ReportScheduleReportsDTO(int reportScheduleReportId, int scheduleId, int reportId, string outputFormat, DateTime lastSuccessfulRunTime)
            : this()
        {
            log.LogMethodEntry( reportScheduleReportId, scheduleId, reportId, outputFormat, lastSuccessfulRunTime);
            this.reportScheduleReportId = reportScheduleReportId;
            this.scheduleId = scheduleId;
            this.reportId = reportId;
            this.outputFormat = outputFormat;
            this.lastSuccessfulRunTime = lastSuccessfulRunTime;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor with all the data fields
        /// </summary>
        public ReportScheduleReportsDTO(int reportScheduleReportId, int scheduleId, int reportId, string guid, int siteId, bool synchStatus, string outputFormat, int masterEntityId, DateTime lastSuccessfulRunTime, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            :this(reportScheduleReportId, scheduleId, reportId, outputFormat, lastSuccessfulRunTime)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdateDate, lastUpdatedBy, guid, siteId, masterEntityId, synchStatus);
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
        /// Get/Set method of the ReportScheduleReportId field
        /// </summary>
        [DisplayName("ReportScheduleReportId")]
        [ReadOnly(true)]
        public int ReportScheduleReportId { get { return reportScheduleReportId; } set { reportScheduleReportId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleId field
        /// </summary>
        [DisplayName("ScheduleId")]
        [ReadOnly(true)]
        public int ScheduleId { get { return scheduleId; } set { scheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReportId field
        /// </summary>
        [DisplayName("ReportId")]
        public int ReportId { get { return reportId; } set { reportId = value; this.IsChanged = true; } }

        
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
        public int SiteId { get { return siteId; } set { siteId = value; } }


        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the OutputFormat field
        /// </summary>
        [DisplayName("OutputFormat")]
        public string OutputFormat { get { return outputFormat; } set { outputFormat = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }//Ends:Modification  on 28-Jun-2016 Passwordhash, passwordsalt,password and masterEntityId


        /// <summary>
        /// Get/Set method of the LastSuccessfulRunTime field
        /// </summary>
        [DisplayName("LastSuccessfulRunTime")]
        public DateTime LastSuccessfulRunTime { get { return lastSuccessfulRunTime; } set { lastSuccessfulRunTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }

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
                    return notifyingObjectIsChanged || reportScheduleReportId < 0;
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
    

