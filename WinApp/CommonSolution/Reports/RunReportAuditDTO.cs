/****************************************************************************************************************
 * Project Name - Report DTO
 * Description  - Data object of user
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *****************************************************************************************************************
 *2.70.2        14-Jul-2019   Dakshakh raj     Modified : Added Parameterized costrustor, Added MasterEntityId field 
 *                                                      and as search parameter
 *****************************************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// RunReportAuditDTO Class
    /// </summary>
    public class RunReportAuditDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByRunReportAuditParameters
        {
            /// <summary>
            /// Search by REPORT ID field
            /// </summary>
            REPORT_ID,

            /// <summary>
            /// Search by START TIME field
            /// </summary>
            START_TIME,

            /// <summary>
            /// Search by END TIME field
            /// </summary>
            END_TIME,

            /// <summary>
            /// Search by CREATED BY field
            /// </summary>
            CREATED_BY,

            /// <summary>
            /// Search by REPORTKEY field
            /// </summary>
            REPORTKEY,

            /// <summary>
            /// Search by MASTERENTITYID field
            /// </summary>
            MASTERENTITYID
        }

        private double runReportAuditId;
        private string reportKey;
        private int reportId;
        private DateTime startTime;
        private DateTime endTime;
        private string parameterList;
        private string message;
        private string source;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private string guid;
        private bool synchstatus;
        private int siteId;
        private int masterEntityId;
       
        /// <summary>
        /// Default constructor
        /// </summary>
        public RunReportAuditDTO()
        {
            log.LogMethodEntry();
            runReportAuditId = -1;
            reportId = -1;
            siteId = -1;
            startTime = DateTime.Now;
            lastUpdateDate = DateTime.Now;
            source = "Run Report";
            message = "Success";
            masterEntityId = -1;
            log.LogMethodExit();

        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public RunReportAuditDTO(double runReportAuditId, string reportKey, int reportId, DateTime startTime, DateTime endTime,
            string parameterList, string message, string source)
            : this()
        {
            log.LogMethodEntry( runReportAuditId,  reportKey,  reportId,  startTime,  endTime, parameterList,  message,  source);
            this.runReportAuditId = runReportAuditId;
            this.reportKey = reportKey;
            this.reportId = reportId;
            this.startTime = startTime;
            this.endTime = endTime;
            this.parameterList = parameterList;
            this.message = message;
            this.source = source;
            log.LogMethodExit();
        }

        /// <summary>
        /// RunReportAuditDTORunReportAuditDTO constructor with All the fields
        /// </summary>
        public RunReportAuditDTO(double runReportAuditId, string reportKey, int reportId, DateTime startTime, DateTime endTime, 
                                 string parameterList, string message, string source, DateTime creationDate, string createdBy, 
                                 DateTime lastUpdateDate, string lastUpdatedBy, string guid, bool synchstatus, int siteId, int masterEntityId)
            :this(runReportAuditId, reportKey, reportId, startTime, endTime, parameterList, message, source)
        {
            log.LogMethodEntry( runReportAuditId,  reportKey,  reportId,  startTime,  endTime, parameterList,  message,  source );
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.synchstatus = synchstatus;
            this.siteId = siteId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the RunReportAuditId field
        /// </summary>
        [DisplayName("Run Report Audit Id")]
        [ReadOnly(true)]
        public double RunReportAuditId { get { return runReportAuditId; } set { runReportAuditId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReportKey field
        /// </summary>
        [DisplayName("Report Key")]
        [ReadOnly(true)]
        public string ReportKey { get { return reportKey; } set { reportKey = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReportId field
        /// </summary>
        [DisplayName("Report Id")]
        [ReadOnly(true)]
        public int ReportId { get { return reportId; } set { reportId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the StartTime field
        /// </summary>
        [DisplayName("Start Time")]
        public DateTime StartTime { get { return startTime; } set { startTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EndTime field
        /// </summary>
        [DisplayName("End Time")]
        public DateTime EndTime { get { return endTime; } set { endTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ParameterList field
        /// </summary>
        [DisplayName("Parameter List")]
        public string ParameterList { get { return parameterList; } set { parameterList = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Message field
        /// </summary>
        [DisplayName("Message")]
        public string Message { get { return message; } set { message = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Source field
        /// </summary>
        [DisplayName("Source")]
        public string Source { get { return source; } set { source = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value;  } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("Last Update Date")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value;  } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchstatus; } set { synchstatus = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value;  } }

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
                    return notifyingObjectIsChanged || runReportAuditId < 0;
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
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }//Ends:Modification  on 28-Jun-2016 Passwordhash, passwordsalt,password and masterEntityId

        /// <summary>
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            {
                log.LogMethodEntry();
                this.IsChanged = false;
                log.LogMethodExit();
            }
        }
    }
}
