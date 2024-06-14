/********************************************************************************************
 * Project Name - ReportParameterValues DTO Programs 
 * Description  - Data object of the ReportParameterValuesDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *1.00        18-April-2017   Rakshith           Created 
 *2.70.2        11-Jul-2019     Dakshakh raj       Modified : Added Parameterized costrustor. Added createdBy,
 *                                                      creationDate fields.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    ///  This is the ReportParameterValuesDTO data object class. This acts as data holder for the Reports business object
    /// </summary>  
    public class ReportParameterValuesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        private int reportParameterValueId;
        private int reportScheduleReportId;
        private int parameterId;
        private string parameterValue;
        private DateTime lastUpdatedDate;
        private string lastUpdatedUser;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by REPORT PARMAMETET VALUE ID field
            /// </summary>
            REPORT_PARMAMETER_VALUE_ID,
            
            /// <summary>
            /// Search by REPORT SCEDULE REPORT ID field
            /// </summary>
            REPORT_SCHEDULE_REPORT_ID,
            
            /// <summary>
            /// Search by PARAMETER ID field
            /// </summary>
            PARAMETER_ID,
            
            /// <summary>
            /// Search by PARAMETER VALUE field
            /// </summary>
            PARAMETER_VALUE,
           
            /// <summary>
            /// Search by SITEID field
            /// </summary>
            SITEID,
            
            /// <summary>
            /// Search by MASTERENTITYID field
            /// </summary>
            MASTERENTITYID 
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportParameterValuesDTO()
        {
            log.LogMethodEntry();
            this.reportParameterValueId = -1;
            this.reportScheduleReportId = -1;
            this.parameterId = -1;
            this.parameterValue = "";
            this.lastUpdatedUser = "";
            this.guid = "";
            this.siteId = -1;
            this.synchStatus = false;
            this.masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public ReportParameterValuesDTO(int reportParameterValueId, int reportScheduleReportId, int parameterId, string parameterValue)
            : this()
        {
            log.LogMethodEntry( reportParameterValueId,  reportScheduleReportId,  parameterId,  parameterValue);
            this.reportParameterValueId = reportParameterValueId;
            this.reportScheduleReportId = reportScheduleReportId;
            this.parameterId = parameterId;
            this.parameterValue = parameterValue;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public ReportParameterValuesDTO(int reportParameterValueId, int reportScheduleReportId, int parameterId, string parameterValue,
                                         DateTime lastUpdatedDate, string lastUpdatedUser, string guid, int siteId, bool synchStatus,
                                         int masterEntityId, string createdBy, DateTime creationDate)
            : this(reportParameterValueId, reportScheduleReportId, parameterId, parameterValue)
        {
            log.LogMethodEntry( lastUpdatedDate,  lastUpdatedUser,  guid,  siteId,  synchStatus, masterEntityId,  createdBy,  creationDate);
            
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ReportName field
        /// </summary>
        [DisplayName("ReportParameterValueId")]
        [DefaultValue(-1)]
        public int ReportParameterValueId { get { return reportParameterValueId; } set { reportParameterValueId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReportScheduleReportId field
        /// </summary>
        [DisplayName("ReportScheduleReportId")]
        [DefaultValue(-1)]
        public int ReportScheduleReportId { get { return reportScheduleReportId; } set { reportScheduleReportId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ParameterId field
        /// </summary>
        [DisplayName("ParameterId")]
        [DefaultValue(-1)]
        public int ParameterId { get { return parameterId; } set { parameterId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ParameterValue field
        /// </summary>
        [DisplayName("ParameterValue")]
        public string ParameterValue { get { return parameterValue; } set { parameterValue = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value;  } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("LastUpdatedUser")]
        public string LastUpdatedUser { get { return lastUpdatedUser; } set { lastUpdatedUser = value;  } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [DefaultValue(-1)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [DefaultValue(-1)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || reportParameterValueId < 0;
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
