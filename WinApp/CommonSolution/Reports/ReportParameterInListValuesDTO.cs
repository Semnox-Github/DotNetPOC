/********************************************************************************************
 * Project Name - ReportParameterInListValues DTO Programs 
 * Description  - Data object of the ReportParameterInListValuesDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *1.00        18-April-2017   Rakshith           Created 
 *2.70.2        12-Jul-2019     Dakshakh raj       Modified : Added Parameterized costrustor.
 *                                                          Added createdBy,creationDate,
 *                                                          lastUpdatedBy,lastUpdateDate fields.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    ///  This is the ReportParameterInListValuesDTO data object class. This acts as data holder for the Reports business object
    /// </summary>  
    public class ReportParameterInListValuesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        private int id;
        private int reportParameterValueId;
        private string inListValue;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

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
            /// Search by IN LIST VALUE field
            /// </summary>
            IN_LIST_VALUE,
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
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
        public ReportParameterInListValuesDTO()
        {
            log.LogMethodEntry();
            this.id = -1;
            this.reportParameterValueId = -1;
            this.InListValue = "";
            this.guid = "";
            this.siteId = -1;
            this.synchStatus = false;
            this.masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public ReportParameterInListValuesDTO(int id, int reportParameterValueId, string inListValue)
            : this()
        {
            log.LogMethodEntry(id, reportParameterValueId, inListValue);
            this.id = id;
            this.reportParameterValueId = reportParameterValueId;
            this.inListValue = inListValue;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public ReportParameterInListValuesDTO(int id, int reportParameterValueId, string inListValue, string guid, int siteId, bool synchStatus,
                                              int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            :this(id, reportParameterValueId, inListValue)
        {
            log.LogMethodEntry(guid, siteId, synchStatus, masterEntityId, createdBy, creationDate,  lastUpdatedBy,lastUpdateDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodEntry();
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        [DefaultValue(-1)]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReportParameterValueId field
        /// </summary>
        [DisplayName("ReportParameterValueId")]
        [DefaultValue(-1)]
        public int ReportParameterValueId { get { return reportParameterValueId; } set { reportParameterValueId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the InListValue field
        /// </summary>
        [DisplayName("InListValue")]
        public string InListValue { get { return inListValue; } set { inListValue = value; this.IsChanged = true; } }

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

