/********************************************************************************************
 * Project Name - ReportSchedule DTO
 * Description  - Data object of ReportSchedule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        20-Apr-2017   Amaresh          Created 
 *2.70.2      10-Jul-2019   Dakshakh raj     Modified(Added parameterized constructor,Added ) 
 *2.90        24-Jul-2020   Laster Menezes   Included new reportschedue field 'MergeReportFiles'
 *2.110       04-Jan-2021   Laster Menezes   Added new reportschedule field 'LastEmailSentDate'
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    ///  This is the ReportSchedule data object class. This acts as data holder for the ReportsSchedule business object
    /// </summary>  
    public class ReportScheduleDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByReportScheduleParameters
        {
            /// <summary>
            /// Search by SCHEDULE ID field
            /// </summary>
            SCHEDULE_ID,

            /// <summary>
            /// Search by SCHEDULE NAME field
            /// </summary>
            SCHEDULE_NAME,

            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,

            /// <summary>
            /// Search by SITE ID field
            /// </summary> 
            SITE_ID,

            /// <summary>
            /// Search by MASTERENTITYID field
            /// </summary>
            MASTERENTITYID,

            /// <summary>
            /// Search by RUNTYPE field
            /// </summary>
            RUNTYPE
        }

        private int scheduleId;
        private string scheduleName;
        private decimal runAt;
        private double includeDataFor;
        private double frequency;
        private string activeFlag;
        private DateTime lastSuccessfulRunTime;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string runType;
        private string triggerQuery;
        private string reportRunning;
        private string mergeReportFiles;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private List<ReportsScheduleEmailDTO> reportsScheduleEmailDTOList;
        private List<ReportScheduleReportsDTO> reportScheduleReportsDTOList;
        private List<ReportScheduleSitesDTO> reportScheduleSitesDTOList;
        private DateTime lastEmailSentDate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportScheduleDTO()
        {
            log.LogMethodEntry();
            scheduleId = -1;
            runAt = 1;
            includeDataFor = 1;
            frequency = -1;
            siteId = -1;
            masterEntityId = -1;
            runType = "Time Event";
            activeFlag = "Y";
            reportRunning = "N";
            mergeReportFiles = "N";
            reportsScheduleEmailDTOList = new List<ReportsScheduleEmailDTO>();
            reportScheduleReportsDTOList = new List<ReportScheduleReportsDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public ReportScheduleDTO(int scheduleId, string scheduleName, decimal runAt, double includeDataFor, double frequency, string activeFlag,
            DateTime lastSuccessfulRunTime, string runType, string triggerQuery, string reportRunning, string mergeReportFiles, DateTime lastEmailSentDate)
            : this()
        {
            log.LogMethodEntry(scheduleId, scheduleName, runAt, includeDataFor, frequency, activeFlag, lastSuccessfulRunTime, runType, triggerQuery, reportRunning, mergeReportFiles, lastEmailSentDate);
            this.scheduleId = scheduleId;
            this.scheduleName = scheduleName;
            this.runAt = runAt;
            this.includeDataFor=includeDataFor;
            this.frequency = frequency;
            this.activeFlag = activeFlag;
            this.lastSuccessfulRunTime = lastSuccessfulRunTime;
            this.runType = runType;
            this.triggerQuery = triggerQuery;
            this.reportRunning = reportRunning;
            this.mergeReportFiles = mergeReportFiles;
            this.reportScheduleReportsDTOList = new List<ReportScheduleReportsDTO>();
            this.reportsScheduleEmailDTOList = new List<ReportsScheduleEmailDTO>();
            this.lastEmailSentDate = lastEmailSentDate;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Parameterized Constructor with all the data fields
        /// </summary>

        public ReportScheduleDTO(int scheduleId, string scheduleName, decimal runAt, double includeDataFor, double frequency, string activeFlag, 
                                 DateTime lastSuccessfulRunTime, string guid, int siteId, bool synchStatus, int masterEntityId, string runType, 
                                 string triggerQuery, string reportRunning,string mergeReportFiles, string createdBy, DateTime creationDate, DateTime lastUpdateDate,
                                 string lastUpdatedBy, DateTime lastEmailSentDate)
            :this(scheduleId, scheduleName, runAt, includeDataFor, frequency, activeFlag, lastSuccessfulRunTime, runType, triggerQuery, reportRunning, mergeReportFiles, lastEmailSentDate)
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
            this.reportScheduleReportsDTOList = new List<ReportScheduleReportsDTO>();
            this.reportsScheduleEmailDTOList = new List<ReportsScheduleEmailDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ScheduleId field
        /// </summary>
        [DisplayName("ScheduleId")]
        [ReadOnly(true)]
        public int ScheduleId { get { return scheduleId; } set { scheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleName field
        /// </summary>
        [DisplayName("ScheduleName")]
        public string ScheduleName { get { return scheduleName; } set { scheduleName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RunAt field
        /// </summary>
        [DisplayName("RunAt")]
        public decimal RunAt { get { return runAt; } set { runAt = value; this.IsChanged = true; } }

        /// <summary>   
        /// Get/Set method of the IncludeDataFor field
        /// </summary>
        [DisplayName("IncludeDataFor")]
        public double IncludeDataFor { get { return includeDataFor; } set { includeDataFor = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Frequency field
        /// </summary>
        [DisplayName("Frequency")]
        public double Frequency { get { return frequency; } set { frequency = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        [DisplayName("ActiveFlag")]
        public string ActiveFlag { get { return activeFlag; } set { activeFlag = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastSuccessfulRunTime field
        /// </summary>
        [DisplayName("LastSuccessfulRunTime")]
        public DateTime LastSuccessfulRunTime { get { return lastSuccessfulRunTime; } set { lastSuccessfulRunTime = value; this.IsChanged = true; } }

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
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RunType field
        /// </summary>
        [DisplayName("RunType")]
        public string RunType { get { return runType; } set { runType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TriggerQuery field
        /// </summary>
        [DisplayName("TriggerQuery")]
        public string TriggerQuery { get { return triggerQuery; } set { triggerQuery = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the ReportRunning field
        /// </summary>
        [DisplayName("ReportRunning")]
        public string ReportRunning { get { return reportRunning; } set { reportRunning = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReportRunning field
        /// </summary>
        [DisplayName("MergeReportFiles")]
        public string MergeReportFiles { get { return mergeReportFiles; } set { mergeReportFiles = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

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
        /// Get/Set method of the LastEmailSentDate field
        /// </summary>
        [DisplayName("Last Email Sent Date")]
        public DateTime LastEmailSentDate { get { return lastEmailSentDate; } set { lastEmailSentDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Returns whether the ReportScheduleEmailDTO changed or any of its ReportScheduleEmailDTO childeren are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (reportsScheduleEmailDTOList != null &&

                   reportsScheduleEmailDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (reportScheduleReportsDTOList != null &&

                  reportScheduleReportsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
                    return notifyingObjectIsChanged || scheduleId < 0;
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}
