/********************************************************************************************
 * Project Name - Reports
 * Description  - Data object of POSMachineReportLog
 *
 **************
 ** Version Log
  **************
  * Version     Date          Modified By            Remarks
 *********************************************************************************************
 *2.70        29-May-2019   Girish Kundar           Created
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Reports
{

    /// <summary>
    /// This is the POSMachineReportLog data object class. This acts as data holder for the POSMachineReportLog business object
    /// </summary>
    public class POSMachineReportLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by REPORT ID field
            /// </summary>
            REPORT_ID,
            /// <summary>
            /// Search by POS MACHINE NAME field
            /// </summary>
            POS_MACHINE_NAME,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by  REPORT SEQUENCE NUMBER field
            /// </summary>
            REPORT_SEQUENCE_NO,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by START TIME field
            /// </summary>
            START_TIME,
            /// <summary>
            /// Search by END TIME field
            /// </summary>
            END_TIME
        }
        private int id;
        private string posMachineName;
        private int reportId;
        private DateTime startTime;
        private DateTime endTime;
        private bool activeFlag ;
        private string reportSequenceNo;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public POSMachineReportLogDTO()
        {
            log.LogMethodEntry();
            id = -1;
            reportId = -1;
            siteId = -1;
            activeFlag = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public POSMachineReportLogDTO(int id, string posMachineName, int reportId, DateTime startTime, DateTime endTime, bool activeFlag, string reportSequenceNo)
            :this()
        {
            log.LogMethodEntry(id, posMachineName, reportId, startTime, endTime, activeFlag, reportSequenceNo);
            this.id = id;
            this.posMachineName = posMachineName;
            this.reportId = reportId;
            this.startTime = startTime;
            this.endTime = endTime;
            this.activeFlag = activeFlag;
            this.reportSequenceNo = reportSequenceNo;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public POSMachineReportLogDTO(int id,string posMachineName, int reportId, DateTime startTime,DateTime endTime,
                                      bool activeFlag, string reportSequenceNo, string guid,int siteId,
                                      bool synchStatus, DateTime lastUpdatedDate,string lastUpdatedBy, 
                                      int masterEntityId, string createdBy, DateTime creationDate )
            :this(id, posMachineName, reportId, startTime, endTime, activeFlag, reportSequenceNo)
        {
            log.LogMethodEntry(id, posMachineName, reportId, startTime, endTime, activeFlag, 
                               reportSequenceNo, guid, siteId, synchStatus, lastUpdatedDate,
                               lastUpdatedBy, masterEntityId, createdBy, creationDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id  field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the posMachineName  field
        /// </summary>
        public string POSMachineName
        {
            get { return posMachineName; }
            set { posMachineName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ReportId  field
        /// </summary>
        public int ReportId
        {
            get { return reportId; }
            set { reportId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the StartTime  field
        /// </summary>
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the EndTime  field
        /// </summary>
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ActiveFlag  field
        /// </summary>
        public bool ActiveFlag
        {
            get { return activeFlag; }
            set { activeFlag = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ReportSequenceNo  field
        /// </summary>
        public string ReportSequenceNo
        {
            get { return reportSequenceNo; }
            set { reportSequenceNo = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value;  }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value;  }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value;  }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value;  }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
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
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            IsChanged = false;
            log.LogMethodExit();
        }


    }
}
