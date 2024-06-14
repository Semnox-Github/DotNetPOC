
/********************************************************************************************
 * Project Name - DisplayGroup
 * Description  - Data object of the ProductDisplayGroupFormat
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        18-May-2016   Amaresh          Created 
 *2.3.0       25-Jun-2018   Guru S A         Rename the class as per db object modifications
 *                                           For User role level product exclusion change 
 *2.70.0      07-Aug-2019   Mushahid Faizan  Added isActive Column.
 *2.90.0      07-Jul-2020   Girish Kundar    Modified: Added master entity Id and 3 tier standard.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.User
{
    public class UserRoleDisplayGroupExclusionsDTO : IChangeTracking
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByDisplayGroupsParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByDisplayGroupsParameters
        {
            /// <summary>
            /// Search by RoleDisplayGroupId field
            /// </summary>
            ROLE_DISPLAY_GROUP_ID,

            /// <summary>
            /// Search by Role Id field
            /// </summary>
            ROLE_ID,
            /// <summary>
            /// Search by site Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Role Id field
            /// </summary>
            ROLE_ID_LIST,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by Master enityID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int roleDisplayGroupId;
        private int roleId;
        private int productDisplayGroupId;
        private string createdBy;
        private DateTime creationDate;
        private DateTime lastUpdatedDate;
        private string lastUpdatedUser;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private bool isActive;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserRoleDisplayGroupExclusionsDTO()
        {
            log.LogMethodEntry();
            siteId = -1;
            productDisplayGroupId = -1;
            roleId = -1;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public UserRoleDisplayGroupExclusionsDTO(int roleDisplayGroupId, int roleId, int productDisplayGroupId, bool isActive)

            : this()
        {
            log.LogMethodEntry(roleDisplayGroupId, roleId, productDisplayGroupId, isActive);
            this.roleDisplayGroupId = roleDisplayGroupId;
            this.roleId = roleId;
            this.productDisplayGroupId = productDisplayGroupId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public UserRoleDisplayGroupExclusionsDTO(int roleDisplayGroupId, int roleId, int productDisplayGroupId, string createdBy, DateTime creationDate, DateTime lastUpdatedDate,
                                            string lastUpdatedUser, int siteId, string guid, bool synchStatus, bool isActive, int masterEntityId)

            : this(roleDisplayGroupId, roleId, productDisplayGroupId, isActive)
        {
            log.LogMethodEntry(roleDisplayGroupId, roleId, productDisplayGroupId, createdBy, creationDate, lastUpdatedDate,
                                             lastUpdatedUser, siteId, guid, synchStatus, isActive, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the id field
        /// </summary>
        [DisplayName("Id")]
        public int RoleDisplayGroupId { get { return roleDisplayGroupId; } set { roleDisplayGroupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RoleId field
        /// </summary>
        [DisplayName("RoleId")]
        public int RoleId { get { return roleId; } set { roleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductDisplay GroupId field
        /// </summary>
        [DisplayName("ProductDisplay GroupId")]
        public int ProductDisplayGroupId { get { return productDisplayGroupId; } set { productDisplayGroupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        [ReadOnly(true)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy ")]
        [ReadOnly(true)]
        public string LastUpdatedBy { get { return lastUpdatedUser; } set { lastUpdatedUser = value; } }

        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        [ReadOnly(true)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the createdBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        [ReadOnly(true)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the siteId field
        /// </summary>
        [Browsable(false)]
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        ///Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; IsChanged = true; } }
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
                    return notifyingObjectIsChanged || roleDisplayGroupId < 0;
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
