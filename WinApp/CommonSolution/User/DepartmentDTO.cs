/********************************************************************************************
 * Project Name - Department DTO
 * Description  - Data object of Department
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.23.4      03-Sep-2018   Indhu                   Created 
 *2.70.2        15-Jul-2019   Girish Kundar           Modified : Added Parametrized Constructor with required fields
 *                                                             and Missed Who columns.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.User
{
    public class DepartmentDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        ///// <summary>
        ///// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        ///// </summary>
        public enum SearchByParameters
        {  
            /// <summary>
            /// Search by DepartmentId
            /// </summary>
            DEPARTMENT_ID,
            /// <summary>
            /// Search by DepartmentName
            /// </summary>
            DEPARTMENT_NAME,
            /// <summary>
            /// Search by ActiveFlag
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by master entity id
            /// </summary>
            MASTER_ENTITY_ID
        }

       private int departmentId;
       private string departmentNumber;
       private string departmentName;
       private string activeFlag;
       private string lastUpdatedBy;
       private DateTime lastUpdatedDate;
       private int site_id;
       private string guid;
       private bool synchStatus;
       private int masterEntityId;
       private string createdBy;
       private DateTime creationDate;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public DepartmentDTO()
        {
            log.LogMethodEntry();
            departmentId = -1;
            departmentNumber = "-1";
            departmentName = string.Empty;
            activeFlag = "Y";
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Constructor  with required parameters.
        /// </summary>
        public DepartmentDTO(int departmentId, string departmentNumber, string departmentName, string activeFlag)
            :this()
        {
            log.LogMethodEntry(departmentId, departmentNumber, departmentName, activeFlag);
            this.departmentId = departmentId;
            this.departmentName = departmentName;
            this.departmentNumber = departmentNumber;
            this.activeFlag = activeFlag;
            log.LogMethodExit();
        }


        /// <summary>
        ///  Constructor  with All the  parameters.
        /// </summary>
        public DepartmentDTO(int departmentId,  string departmentNumber,  string departmentName,  string activeFlag,
                             string lastUpdatedBy, DateTime lastUpdatedDate, int site_id, string guid,  bool synchStatus,
                             int masterEntityId, string createdBy,DateTime creationDate)
            :this(departmentId, departmentNumber, departmentName, activeFlag)
        {
            log.LogMethodEntry(departmentId, departmentNumber, departmentName,
                               activeFlag, LastUpdatedBy, LastUpdatedDate,
                               site_id, guid, synchStatus, masterEntityId, createdBy, creationDate);
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the departmentId field
        /// </summary>
        [DisplayName("departmentId")]
        public int DepartmentId
        {
            get
            {
                return departmentId;
            }

            set
            {
                this.IsChanged = true;
                departmentId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the departmentName field
        /// </summary>
        [DisplayName("departmentName")]
        public string DepartmentName
        {
            get
            {
                return departmentName;
            }

            set
            {
                this.IsChanged = true;
                departmentName = value;
            }
        }
        /// <summary>
        /// Get/Set method of the departmentNumber field
        /// </summary>
        [DisplayName("departmentNumber")]
        public string DepartmentNumber
        {
            get
            {
                return departmentNumber;
            }

            set
            {
                this.IsChanged = true;
                departmentNumber = value;
            }
        }
        /// <summary>
        /// Get/Set method of the activeFlag field
        /// </summary>
        [DisplayName("activeFlag")]
        public string ActiveFlag
        {
            get
            {
                return activeFlag;
            }

            set
            {
                this.IsChanged = true;
                activeFlag = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate
        {
            get
            {
                return lastUpdatedDate;
            }
            set
            {
               
                lastUpdatedDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
               
                lastUpdatedBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        public int Site_Id
        {
            get
            {
                return site_id;
            }
            set
            {
              
                site_id = value;
            }
        }
        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        public string GUID
        {
            get
            {
                return guid;
            }
            set
            {
                this.IsChanged = true;
                guid = value;
            }
        }
        /// <summary>
        /// Get/Set method of the synchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
               
                synchStatus = value;
            }
        }
        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>

        public string CreatedBy    { get   {  return createdBy;  } set   {  createdBy = value;   }    }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
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
                    return notifyingObjectIsChanged || departmentId < 0;
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
