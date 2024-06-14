/********************************************************************************************
 * Project Name - Concurrent Programs Parameters DTO
 * Description  - DTO class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.120.1       26-Apr-2021   Deeksha             Created as part of AWS Concurrent Programs enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.JobUtils
{
    public class ConcurrentProgramParametersDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by Concurrent Program Parameter Id
            /// </summary>
            CONCURRENT_PROGRAM_PARAMETER_ID,

            /// <summary>
            /// Search by Program Id
            /// </summary>
            PROGRAM_ID,

            /// <summary>
            /// Search by is active
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int concurrentProgramParameterId;
        private int programId;
        private string parameterName;
        private string sqlParameter;
        private string parameterDescription;
        private string dataType;
        private string dataSource;
        private string dataSourceType;
        private string parameterOperator;
        private int displayOrder;
        private bool mandatory;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId;
        private List<KeyValuePair<string, string>> concProgramValueList;


        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <returns></returns>
        public ConcurrentProgramParametersDTO()
        {
            log.LogMethodEntry();
            concurrentProgramParameterId = -1;
            programId = -1;
            parameterName = string.Empty;
            sqlParameter = string.Empty;
            parameterDescription = string.Empty;
            dataType = string.Empty;
            dataSourceType = string.Empty;
            parameterOperator = string.Empty;
            isActive = true;
            masterEntityId = -1;
            siteId = -1;
            concProgramValueList = new List<KeyValuePair<string, string>>();
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameter
        /// </summary>
        public ConcurrentProgramParametersDTO(int concurrentProgramParameterId, int programId, string parameterName, string sqlParameter,
                                              string parameterDescription, string dataType, string dataSource, string dataSourceType, string parameterOperator,
                                              int displayOrder, bool mandatory, bool isActive)
            : this()
        {
            log.LogMethodEntry(concurrentProgramParameterId, programId, parameterName, sqlParameter, parameterDescription, dataType, dataSource,
                               dataSourceType, parameterOperator, displayOrder, mandatory, isActive);
            this.concurrentProgramParameterId = concurrentProgramParameterId;
            this.programId = programId;
            this.parameterName = parameterName;
            this.sqlParameter = sqlParameter;
            this.parameterDescription = parameterDescription;
            this.dataType = dataType;
            this.dataSource = dataSource;
            this.dataSourceType = dataSourceType;
            this.parameterOperator = parameterOperator;
            this.displayOrder = displayOrder;
            this.mandatory = mandatory;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ConcurrentProgramParametersDTO(int concurrentProgramParameterId, int programId, string parameterName, string sqlParameter,
                                              string parameterDescription, string dataType, string dataSource, string dataSourceType, string parameterOperator,
                                              int displayOrder, bool mandatory, bool isActive, string createdBy, DateTime creationDate,
                                               string lastUpdatedBy, DateTime lastUpdatedDate, string guid, bool synchStatus,
                                               int siteId, int masterEntityId)
            : this(concurrentProgramParameterId, programId, parameterName, sqlParameter, parameterDescription, dataType, dataSource,
                               dataSourceType, parameterOperator, displayOrder, mandatory, isActive)
        {
            log.LogMethodEntry(concurrentProgramParameterId, programId, parameterName, sqlParameter, parameterDescription, dataType, dataSource,
                               dataSourceType, parameterOperator, displayOrder, mandatory, isActive, createdBy, creationDate,
                               lastUpdatedBy, lastUpdatedDate, guid, siteId, masterEntityId, synchStatus);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ConcurrentProgramParameterId field
        /// </summary>
        public int ConcurrentProgramParameterId { get { return concurrentProgramParameterId; } set { this.IsChanged = true; concurrentProgramParameterId = value; } }


        /// <summary>
        /// Get/Set method of the ProgramId field
        /// </summary>
        public int ProgramId { get { return programId; } set { this.IsChanged = true; programId = value; } }

        /// <summary>
        /// Get/Set method of the ParameterName field
        /// </summary>
        public string ParameterName { get { return parameterName; } set { this.IsChanged = true; parameterName = value; } }

        /// <summary>
        /// Get/Set method of the SQLParameter field
        /// </summary>
        public string SQLParameter { get { return sqlParameter; } set { this.IsChanged = true; sqlParameter = value; } }

        /// <summary>
        /// Get/Set method of the ParameterDescription field
        /// </summary>
        public string ParameterDescription { get { return parameterDescription; } set { this.IsChanged = true; parameterDescription = value; } }

        /// <summary>
        /// Get/Set method of the DataType field
        /// </summary>
        public string DataType { get { return dataType; } set { this.IsChanged = true; dataType = value; } }

        /// <summary>
        /// Get/Set method of the DataSource field
        /// </summary>
        public string DataSource { get { return dataSource; } set { this.IsChanged = true; dataSource = value; } }


        /// <summary>
        /// Get/Set method of the DataSourceType field
        /// </summary>
        public string DataSourceType { get { return dataSourceType; } set { this.IsChanged = true; dataSourceType = value; } }

        /// <summary>
        /// Get/Set method of the Operator field
        /// </summary>
        public string Operator { get { return parameterOperator; } set { this.IsChanged = true; parameterOperator = value; } }

        /// <summary>
        /// Get/Set method of the DisplayOrder field
        /// </summary>
        public int DisplayOrder { get { return displayOrder; } set { this.IsChanged = true; displayOrder = value; } }

        /// <summary>
        /// Get/Set method of the Mandatory field
        /// </summary>
        public bool Mandatory { get { return mandatory; } set { this.IsChanged = true; mandatory = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

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
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }


        /// <summary>
        /// Get/Set method of the SegmentValueList field
        /// </summary>
        public List<KeyValuePair<string,string>> ProgramValueList { get { return concProgramValueList; } set { concProgramValueList = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || concurrentProgramParameterId < 0;
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
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}