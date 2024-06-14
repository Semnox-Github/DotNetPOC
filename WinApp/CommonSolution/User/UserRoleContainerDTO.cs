/********************************************************************************************
 * Project Name - Users
 * Description  - Data structure of the UserRoleViewContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 2.130.0        12-Jul-2021    Lakshminarayana      Modified : Static menu enhancement
 ********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Data structure of the UserRoleViewContainer class 
    /// </summary>
    public class UserRoleContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int roleId;
        private string role;
        private string description;
        private bool manager;
        private string allowPosAccess;
        private string dataAccessLevel;
        private bool enablePOSClockIn;
        private int assignedManagerRoleId;
        private bool allowShiftOpenClose;
        private bool isManagerRole;
        private int priceListId;
        private List<int> assignedManagerRoleIdList = new List<int>();
        private List<int> excludedProductIdList = new List<int>();
        private List<ManagementFormAccessContainerDTO> managementFormAccessContainerDTOList;
        private List<int> excludedProductMenuPanelIdList = new List<int>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserRoleContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all fields
        /// </summary>
        public UserRoleContainerDTO(int roleId, string role, string description, bool manager,
            string allowPosAccess, string dataAccessLevel, bool enablePOSClockIn, bool allowShiftOpenClose, 
            bool isManagerRole, int priceListId)
        {
            log.LogMethodEntry(roleId, role, description, manager, allowPosAccess, dataAccessLevel, enablePOSClockIn, allowShiftOpenClose, isManagerRole, priceListId);
            this.roleId = roleId;
            this.role = role;
            this.description = description;
            this.manager = manager;
            this.allowPosAccess = allowPosAccess;
            this.dataAccessLevel = dataAccessLevel;
            this.enablePOSClockIn = enablePOSClockIn;
            this.allowShiftOpenClose = allowShiftOpenClose;
            this.isManagerRole = isManagerRole;
            this.priceListId = priceListId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of roleId field
        /// </summary>
        public int RoleId
        {
            get
            {
                return roleId;
            }
            set
            {
                roleId = value;
            }
        }

        /// <summary>
        /// Get/Set method of role field 
        /// </summary>
        public string Role
        {
            get
            {
                return role;
            }
            set
            {
                role = value;
            }
        }

        /// <summary>
        /// Get/Set method of description field 
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        /// <summary>
        /// Get/Set method of manager field 
        /// </summary>
        public bool Manager
        {
            get
            {
                return manager;
            }
            set
            {
                manager = value;
            }
        }

        /// <summary>
        /// Get/Set method of allowPosAccess field 
        /// </summary>
        public string AllowPosAccess
        {
            get
            {
                return allowPosAccess;
            }
            set
            {
                allowPosAccess = value;
            }
        }

        /// <summary>
        /// Get/Set method of dataAccessLevel field 
        /// </summary>
        public string DataAccessLevel
        {
            get
            {
                return dataAccessLevel;
            }
            set
            {
                dataAccessLevel = value;
            }
        }

        /// <summary>
        /// Get/Set method of enablePOSClockIn field 
        /// </summary>
        public bool EnablePOSClockIn
        {
            get
            {
                return enablePOSClockIn;
            }
            set
            {
                enablePOSClockIn = value;
            }
        }

        /// <summary>
        /// Get/Set method of allowShiftOpenClose field 
        /// </summary>
        public bool AllowShiftOpenClose
        {
            get
            {
                return allowShiftOpenClose;
            }
            set
            {
                allowShiftOpenClose = value;
            }
        }

        /// <summary>
        /// Get/Set method of isManagerRole field 
        /// </summary>
        public bool IsManagerRole
        {
            get
            {
                return isManagerRole;
            }
            set
            {
                isManagerRole = value;
            }
        }

        /// <summary>
        /// Get/Set method of assignedManagerRoleId field
        /// </summary>
        public int AssignedManagerRoleId
        {
            get
            {
                return assignedManagerRoleId;
            }
            set
            {
                assignedManagerRoleId = value;
            }
        }

        /// <summary>
        /// Get/Set method of priceListId field
        /// </summary>
        public int PriceListId
        {
            get
            {
                return priceListId;
            }
            set
            {
                priceListId = value;
            }
        }

        /// <summary>
        /// Get/Set method of managementFormAccessContainerDTOList field 
        /// </summary>
        public List<ManagementFormAccessContainerDTO> ManagementFormAccessContainerDTOList
        {
            get
            {
                return managementFormAccessContainerDTOList;
            }
            set
            {
                managementFormAccessContainerDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of assignedManagerRoleIdList field 
        /// </summary>
        public List<int> AssignedManagerRoleIdList
        {
            get
            {
                return assignedManagerRoleIdList;
            }
            set
            {
                assignedManagerRoleIdList = value;
            }
        }

        /// <summary>
        /// Get/Set method of ExcludedRoleIdList field 
        /// </summary>
        public List<int> ExcludedProductIdList
        {
            get
            {
                return excludedProductIdList;
            }
            set
            {
                excludedProductIdList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the exludedProductMenuPanelIdList field
        /// </summary>
        public List<int> ExcludedProductMenuPanelIdList
        {
            get
            {
                return excludedProductMenuPanelIdList;
            }

            set
            {
                excludedProductMenuPanelIdList = value;
            }
        }
    }
}
