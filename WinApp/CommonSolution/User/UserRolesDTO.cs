/********************************************************************************************
 * Project Name - User Roles DTO
 * Description  - Data object of user roles  
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        28-Jun-2016   Raghuveera      Created 
 *2.00        04-Mar-2019   Indhu           Modified for Remote Shift Open/Close changes
 *2.70.2        15-Jul -2019  Girish Kundar  Modified : Added Parametrized Constructor with required fields
 *                                                     And Missed Who columns
 *2.70.0      05-Aug-2019   Mushahid Faizan Added IsActive   
 *2.90.0     09-Jul-2020   Akshay Gulaganji Modified : Added field ShiftConfigurationId
 *2.130.0        12-Jul-2021    Lakshminarayana      Modified : Static menu enhancement
 ********************************************************************************************/
using Semnox.Parafait.PriceList;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.User
{
    /// <summary>
    ///  This is the user role data object class. This acts as data holder for the user roles business object
    /// </summary>
    public class UserRolesDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchBySiteParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByUserRolesParameters
        {
            /// <summary>
            /// Search by ROLE_ID field
            /// </summary>
            ROLE_ID,
            /// <summary>
            /// Search by ROLE field
            /// </summary>
            ROLE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by ASSIGNED_MANAGER_ROLEID field
            /// </summary>
            ASSIGNED_MANAGER_ROLEID,
            ///<summary>
            ///Search by ALLOW_SHIFT_OPEN_CLOSE
            ///</summary>
            ALLOW_SHIFT_OPEN_CLOSE,
            ///<summary>
            ///Search by is Active
            ///</summary>
            ISACTIVE,
            ///<summary>
            ///Search by SHIFT CONFIGURATION ID
            ///</summary>
            SHIFT_CONFIGURATION_ID,
            /// <summary>
            /// Search by ROLE ID LIST
            /// </summary>
            ROLE_ID_LIST,
            /// <summary>
            /// Search by ROLE ID Exclusion LIST
            /// </summary>
            ROLE_NAME_EXCLUSION_LIST,
            /// <summary>
            /// Search by ENABLE_POS_CLOCKIN
            /// </summary>
            ENABLE_POS_CLOCKIN
        }

        private int roleId;
        private string role;
        private string description;
        private string managerFlag;
        private string allowPosAccess;
        private string dataAccessLevel;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private bool enablePOSClockIn;
        private bool allowShiftOpenClose;
        private int securityPolicyId;
        private int masterEntityId;
        private int assignedManagerRoleId;
        private int dataAccessRuleId;
        private List<UsersDTO> usersDTOList;
        private string createdBy;
        private bool isActive;
        private DateTime creationDate;
        private List<UserRoleDisplayGroupExclusionsDTO> userRoleDisplayGroupExclusionsDTOList;
        private List<ProductMenuPanelExclusionDTO> productMenuPanelExclusionDTOList;
        private List<UserRolePriceListDTO> userRolePriceListDTOList;
        private int shiftConfigurationId;
        private List<ManagementFormAccessDTO> managementFormAccessDTOList;
        /// <summary>
        /// Default constructor
        /// </summary>
        public UserRolesDTO()
        {
            log.LogMethodEntry();
            roleId = -1;
            siteId = -1;
            securityPolicyId = -1;
            masterEntityId = -1;
            assignedManagerRoleId = -1;
            dataAccessRuleId = -1;
            shiftConfigurationId = -1;
            isActive = true;
            usersDTOList = new List<UsersDTO>();
            productMenuPanelExclusionDTOList = new List<ProductMenuPanelExclusionDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        ///  </summary>
        public UserRolesDTO(int roleId, string role, string description, string managerFlag, string allowPosAccess,
                           string dataAccessLevel, bool enablePOSClockIn, bool allowShiftOpenClose, int securityPolicyId,
                                          int assignedManagerRoleId, int dataAccessRuleId, int shiftConfigurationId)
           : this()
        {
            log.LogMethodEntry(roleId, role, description, managerFlag, allowPosAccess,
                               dataAccessLevel, enablePOSClockIn, allowShiftOpenClose, securityPolicyId,
                                assignedManagerRoleId, dataAccessRuleId, shiftConfigurationId);
            this.roleId = roleId;
            this.role = role;
            this.description = description;
            this.managerFlag = managerFlag;
            this.allowPosAccess = allowPosAccess;
            this.dataAccessLevel = dataAccessLevel;
            this.enablePOSClockIn = enablePOSClockIn;
            this.allowShiftOpenClose = allowShiftOpenClose;
            this.securityPolicyId = securityPolicyId;
            this.assignedManagerRoleId = assignedManagerRoleId; //Added on 15-Dec-2016
            this.dataAccessRuleId = dataAccessRuleId;
            this.shiftConfigurationId = shiftConfigurationId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public UserRolesDTO(int roleId, string role, string description, string managerFlag, string allowPosAccess,
                            string dataAccessLevel, string guid, int siteId, bool synchStatus, string lastUpdatedBy,
                            DateTime lastUpdatedDate, bool enablePOSClockIn, bool allowShiftOpenClose, int securityPolicyId,
                                      int masterEntityId, int assignedManagerRoleId, int dataAccessRuleId, string createdBy, DateTime creationDate, bool isActive, int shiftConfigurationId)
            : this(roleId, role, description, managerFlag, allowPosAccess,
                  dataAccessLevel, enablePOSClockIn, allowShiftOpenClose, securityPolicyId,
                  assignedManagerRoleId, dataAccessRuleId, shiftConfigurationId)
        {
            log.LogMethodEntry(roleId, role, description, managerFlag, allowPosAccess,
                             dataAccessLevel, guid, siteId, synchStatus, lastUpdatedBy,
                             lastUpdatedDate, enablePOSClockIn, allowShiftOpenClose, securityPolicyId,
                                   masterEntityId, assignedManagerRoleId, dataAccessRuleId, createdBy, creationDate, shiftConfigurationId);

            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the RoleId field
        /// </summary>
        [DisplayName("Role Id")]
        [ReadOnly(true)]
        public int RoleId { get { return roleId; } set { roleId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Role field
        /// </summary>
        [DisplayName("Role")]
        public string Role { get { return role; } set { role = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ManagerFlag field
        /// </summary>
        [DisplayName("IsManager?")]
        public string ManagerFlag { get { return managerFlag; } set { managerFlag = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AllowPosAccess field
        /// </summary>
        [DisplayName("AllowPosAccess?")]
        public string AllowPosAccess { get { return allowPosAccess; } set { allowPosAccess = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DataAccessLevel field
        /// </summary>
        [DisplayName("Data Access Level")]
        public string DataAccessLevel { get { return dataAccessLevel; } set { dataAccessLevel = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }
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
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        /// [Browsable(false)]
        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the EnablePOSClockIn field
        /// </summary>
        [DisplayName("EnablePOSClockIn?")]
        public bool EnablePOSClockIn { get { return enablePOSClockIn; } set { enablePOSClockIn = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AllowShiftOpenClose field
        /// </summary>
        [DisplayName("AllowShiftOpenClose?")]
        public bool AllowShiftOpenClose { get { return allowShiftOpenClose; } set { allowShiftOpenClose = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SecurityPolicyId field
        /// </summary>
        [DisplayName("Security Policy")]
        public int SecurityPolicyId { get { return securityPolicyId; } set { securityPolicyId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AssignedManagerRoleId field
        /// </summary>
        [DisplayName("AssignedManagerRoleId")]
        [ReadOnly(true)]
        public int AssignedManagerRoleId { get { return assignedManagerRoleId; } set { assignedManagerRoleId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DataAccessRuleId field
        /// </summary>
        [DisplayName("Data Access Rule")]
        public int DataAccessRuleId { get { return dataAccessRuleId; } set { dataAccessRuleId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get/Set method of the ShiftConfigurationId field
        /// </summary>
        [DisplayName("Shift Configuration Id")]
        public int ShiftConfigurationId { get { return shiftConfigurationId; } set { shiftConfigurationId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set methods for UserIdentificationTagsDTO 
        /// </summary>
        public List<UsersDTO> UsersDTO
        {
            get
            {
                return usersDTOList;
            }

            set
            {
                usersDTOList = value;
            }
        }
        public List<UserRoleDisplayGroupExclusionsDTO> UserRoleDisplayGroupExclusionsDTOList
        {
            get
            {
                return userRoleDisplayGroupExclusionsDTOList;
            }

            set
            {
                userRoleDisplayGroupExclusionsDTOList = value;
            }
        }
        public List<UserRolePriceListDTO> UserRolePriceListDTOList
        {
            get
            {
                return userRolePriceListDTOList;
            }

            set
            {
                userRolePriceListDTOList = value;
            }
        }
        public List<ManagementFormAccessDTO> ManagementFormAccessDTOList
        {
            get
            {
                return managementFormAccessDTOList;
            }

            set
            {
                managementFormAccessDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the productMenuPanelExclusionDTOList field
        /// </summary>
        [Browsable(false)]
        public List<ProductMenuPanelExclusionDTO> ProductMenuPanelExclusionDTOList
        {
            get
            {
                return productMenuPanelExclusionDTOList;
            }

            set
            {
                productMenuPanelExclusionDTOList = value;
            }
        }


        /// <summary>
        /// Returns whether the UserRolesDTO changed or any of its usersDTOList  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (usersDTOList != null &&
                   usersDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (managementFormAccessDTOList != null &&
                   managementFormAccessDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (userRolePriceListDTOList != null &&
                   userRolePriceListDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (userRoleDisplayGroupExclusionsDTOList != null &&
                   userRoleDisplayGroupExclusionsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (productMenuPanelExclusionDTOList != null &&
                    productMenuPanelExclusionDTOList.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || roleId < 0 ;
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
