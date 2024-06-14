
/********************************************************************************************
 * Project Name - DisplayGroup
 * Description  - Data object of the ProductDisplayGroupFormat
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        18-May-2016    Amaresh          Created 
 *2.110.0     03-Dec-2020    Prajwal S       Modified three tier
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class UserRoleDisplayGroupsDTO : IChangeTracking
    {
       private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    
    /// <summary>
    /// SearchByDisplayGroupsParameters enum controls the search fields, this can be expanded to include additional fields
    /// </summary>
    public enum SearchByDisplayGroupsParameters
    {
        /// <summary>
        /// Search by RoleDisplayGroupId field
        /// </summary>
        ROLE_DISPLAY_GROUP_ID = 0,

        /// <summary>
        /// Search by Role Id field
        /// </summary>
        ROLE_ID = 1,

        /// </summary>
         /// Search by IsActive field
         /// </summary>
       IS_ACTIVE,

        /// <summary>
         /// Search by site_id field
        /// </summary>
        SITE_ID,

       /// <summary>
       /// Search by Master entity id field
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
        private int masterEntityId;  //added
        private bool isActive;  //added

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserRoleDisplayGroupsDTO()
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
        public UserRoleDisplayGroupsDTO(int roleDisplayGroupId, int roleId, int productDisplayGroupId, bool isActive)
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
        public UserRoleDisplayGroupsDTO(int roleDisplayGroupId, int roleId, int productDisplayGroupId, string createdBy, DateTime creationDate,
                                        DateTime lastUpdatedDate, string lastUpdatedUser, int siteId, string guid, bool synchStatus, int masterEntityId, bool isActive)
            :this(roleDisplayGroupId, roleId, productDisplayGroupId, isActive)                                     
        {
            log.LogMethodEntry(roleDisplayGroupId, roleId, productDisplayGroupId, createdBy, creationDate, lastUpdatedDate,
                               lastUpdatedUser, siteId, guid, synchStatus, masterEntityId, isActive);
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
        public int RoleDisplayGroupId { get { return roleDisplayGroupId; } set { roleDisplayGroupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RoleId field
        /// </summary>
        public int RoleId { get { return roleId; } set { roleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductDisplay GroupId field
        /// </summary>
        public int ProductDisplayGroupId { get { return productDisplayGroupId; } set { productDisplayGroupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedUser; } set { lastUpdatedUser = value; } }

        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value;  } }

        /// <summary>
        /// Get/Set method of the createdBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }

        /// <summary>
        /// Get/Set method of the siteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value;  } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true;  } }    

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
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
