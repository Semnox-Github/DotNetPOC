/********************************************************************************************
 * Project Name - Report ParametersDTO
 * Description  - Data object of Report ParametersDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        17-Apr-2017   Amaresh          Created 
 *2.70.2        11-Jul-2019   Dakshakh raj     Modified : Added Parameterized costrustor. Added createdBy,
 *                                                      creationDate fields.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    ///  This is the Reports Parameter data object class. This acts as data holder for the Reports parameter business object
    /// </summary>
    public class ReportParametersDTO
        {
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            private bool notifyingObjectIsChanged;
            private readonly object notifyingObjectIsChangedSyncRoot = new Object();

            /// <summary>
            /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
            /// </summary>
            public enum SearchByReportsParameters
            {
            /// <summary>
            /// Search by PARAMETER ID field
            /// </summary>
            PARAMETER_ID,

            /// <summary>
            /// Search by REPORT ID field
            /// </summary>
            REPORT_ID,

            /// <summary>
            /// Search by PARAMETER NAME field
            /// </summary>
            PARAMETER_NAME,

            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,

            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,

            /// <summary>
            /// Search by MASTERENTITYID field
            /// </summary>
            MASTERENTITYID
        }

        private int parameterId;
        private int reportId;
        private string parameterName;
        private string sqlParameter;
        private string description;
        private string dataType;
        private string dataSourceType;
        private string dataSource;
        private string operater;
        private bool activeFlag;
        private int displayOrder;
        private bool mandatory;
        private DateTime lastUpdatedDate;
        private string lastUpdatedUser;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportParametersDTO()
            {
                log.LogMethodEntry();
                reportId = -1;
                parameterId = -1;
                displayOrder = -1;
                siteId = -1;
                masterEntityId = -1;
                log.LogMethodExit();
        }

    /// <summary>
    /// Parameterized constructor with Required fields
    /// </summary>
    public ReportParametersDTO(int parameterId, int reportId, string parameterName, string sqlParameter, string description, string dataType,
                    string dataSourceType, string dataSource, string operater, int displayOrder, bool mandatory, bool activeFlag)
        : this()
    {
        log.LogMethodEntry( parameterId,  reportId,  parameterName,  sqlParameter,  description,  dataType,
                            dataSourceType,  dataSource,  operater,  displayOrder,  mandatory, activeFlag);
        this.parameterId = parameterId;
        this.reportId = reportId;
        this.parameterName = parameterName;
        this.sqlParameter = sqlParameter;
        this.description = description;
        this.dataType = dataType;
        this.dataSourceType = dataSourceType;
        this.dataSource = dataSource;
        this.operater = operater;
        this.displayOrder = displayOrder;
        this.mandatory = mandatory;
        this.activeFlag = activeFlag;
        log.LogMethodExit();
    }

        /// <summary>
        /// Parameterized constructor with all the data fields
        /// </summary>
        public ReportParametersDTO(int parameterId, int reportId, string parameterName, string sqlParameter, string description, string dataType,
                                   string dataSourceType, string dataSource, string operater, int displayOrder, bool mandatory,  DateTime lastUpdatedDate,
                                   string lastUpdatedUser, string guid, int siteId, bool synchStatus, bool activeFlag, int masterEntityId, string createdBy, DateTime creationDate)
            :this(parameterId, reportId, parameterName, sqlParameter, description, dataType, dataSourceType, dataSource, operater, displayOrder, mandatory, activeFlag)
            {
            log.LogMethodEntry(createdBy, creationDate, guid, siteId, masterEntityId, synchStatus, activeFlag, lastUpdatedDate, lastUpdatedUser);
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
            /// Get/Set method of the reportId field
            /// </summary>
            [DisplayName("reportId")]
            [ReadOnly(true)]
            public int ParameterId { get { return parameterId; } set { parameterId = value; this.IsChanged = true; } }

            /// <summary>
            /// Get/Set method of the reportId field
            /// </summary>
            [DisplayName("reportId")]
            public int ReportId { get { return reportId; } set { reportId = value; this.IsChanged = true; } }

            /// <summary>
            /// Get/Set method of the ParameterName field
            /// </summary>
            [DisplayName("ParameterName")]
            public string ParameterName { get { return parameterName; } set { parameterName = value; this.IsChanged = true; } }

            /// <summary>
            /// Get/Set method of the SqlParameter field
            /// </summary>
            [DisplayName("SqlParameter")]
            public string SqlParameter { get { return sqlParameter; } set { sqlParameter = value; this.IsChanged = true; } }

             /// <summary>
             /// Get/Set method of the description field
             /// </summary>
             [DisplayName("Description")]
                 public string Description { get { return description; } set { description = value; this.IsChanged = true; } }
            
             /// <summary>
             /// Get/Set method of the DataType field
             /// </summary>
             [DisplayName("DataType")]
                 public string DataType { get { return dataType; } set { dataType = value; this.IsChanged = true; } }
            
             /// <summary>
             /// Get/Set method of the DataSourceType field
             /// </summary>
             [DisplayName("DataSourceType")]
                 public string DataSourceType { get { return dataSourceType; } set { dataSourceType = value; this.IsChanged = true; } }
            
             /// <summary>
             /// Get/Set method of the DataSource field
             /// </summary>
             [DisplayName("DataSource")]
             public string DataSource { get { return dataSource; } set { dataSource = value; this.IsChanged = true; } }
            
             /// <summary>
             /// Get/Set method of the Operater field
             /// </summary>
             [DisplayName("Operater")]
            public string Operator { get { return operater; } set { operater = value; this.IsChanged = true; } }

            /// <summary>
            /// Get/Set method of the ActiveFlag field
            /// </summary>
            [DisplayName("ActiveFlag")]
            public bool ActiveFlag { get { return activeFlag; } set { activeFlag = value; this.IsChanged = true; } }

            /// <summary>
            /// Get/Set method of the DisplayOrder field
            /// </summary>
            [DisplayName("DisplayOrder")]
            public int DisplayOrder { get { return displayOrder; } set { displayOrder = value; this.IsChanged = true; } }

            /// <summary>
            /// Get/Set method of the Mandatory field
            /// </summary>
            [DisplayName("Mandatory")]
            public bool Mandatory { get { return mandatory; } set { mandatory = value; this.IsChanged = true; } }

            /// <summary>
            /// Get/Set method of the LastUpdatedDate field
            /// </summary>
            [DisplayName("LastUpdatedDate")]
            public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

            /// <summary>
            /// Get/Set method of the LastUpdatedUser field
            /// </summary>
            [DisplayName("LastUpdatedUser")]
            public string LastUpdatedUser { get { return lastUpdatedUser; } set { lastUpdatedUser = value; } }

            /// <summary>
            /// Get/Set method of the Guid field
            /// </summary>
            [DisplayName("Guid")]
            [Browsable(false)]
            public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

            /// <summary>
            /// Get/Set method of the SiteId field
            /// </summary>
            [DisplayName("Site Id")]
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
            public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }//Ends:Modification  on 28-Jun-2016 Passwordhash, passwordsalt,password and masterEntityId


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
                        return notifyingObjectIsChanged || parameterId < 0;
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
