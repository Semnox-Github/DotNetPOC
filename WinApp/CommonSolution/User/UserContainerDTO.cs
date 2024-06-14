/********************************************************************************************
 * Project Name - Users
 * Description  - Data structure of the UserViewContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
*2.140.0      01-Jun-2021      Fiona Lishal              Modified for Delivery Order enhancements for F&B
 ********************************************************************************************/

using System.Collections.Generic;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Data structure of the UserViewContainer class 
    /// </summary>
    public class UserContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int userId;
        private string userName;
        private string empLastName;
        private string loginId;
        private int roleId;
        private int managerId;
        private int siteId;
        private int posTypeId;
        private string guid;
        private bool selfApprovalAllowed;
        private string phoneNumber;
        private List<UserIdentificationTagContainerDTO> userIdentificationTagContainerDTOList = new List<UserIdentificationTagContainerDTO>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all fields
        /// </summary>
        public UserContainerDTO(int userId, string userName,string empLastName, string loginId, int roleId, int managerId, int siteId, int posTypeId, string guid, bool selfApprovalAllowed,string phoneNumber)
        {
            log.LogMethodEntry(userId, userName, loginId, roleId, siteId, posTypeId, guid, selfApprovalAllowed);
            this.userId = userId;
            this.empLastName = empLastName;
            this.userName = userName;
            this.loginId = loginId;
            this.roleId = roleId;
            this.managerId = managerId;
            this.siteId = siteId;
            this.posTypeId = posTypeId;
            this.guid = guid;
            this.selfApprovalAllowed = selfApprovalAllowed;
            this.phoneNumber = phoneNumber;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of userId field
        /// </summary>
        public int UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
            }
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
        /// Get/Set method of userName field 
        /// </summary>
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }
        /// <summary>
        /// Get/Set method of empLastName field 
        /// </summary>
        public string EmpLastName
        {
            get
            {
                return empLastName;
            }
            set
            {
                empLastName = value;
            }
        }
        /// <summary>
        /// Get/Set method of description field 
        /// </summary>
        public string LoginId
        {
            get
            {
                return loginId;
            }
            set
            {
                loginId = value;
            }
        }

        /// <summary>
        /// Get/Set method of managerId field 
        /// </summary>
        public int ManagerId
        {
            get
            {
                return managerId;
            }
            set
            {
                managerId = value;
            }
        }

        /// <summary>
        /// Get/Set method of siteId field 
        /// </summary>
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                siteId = value;
            }
        }

        /// <summary>
        /// Get/Set method of posTypeId field 
        /// </summary>
        public int POSTypeId
        {
            get
            {
                return posTypeId;
            }
            set
            {
                posTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of selfApprovalAllowed field 
        /// </summary>
        public bool SelfApprovalAllowed
        {
            get
            {
                return selfApprovalAllowed;
            }
            set
            {
                selfApprovalAllowed = value;
            }
        }

        /// <summary>
        /// Get/Set method of guid field 
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method of userIdentificationTagContainerDTOList field 
        /// </summary>
        public List<UserIdentificationTagContainerDTO> UserIdentificationTagContainerDTOList
        {
            get
            {
                return userIdentificationTagContainerDTOList;
            }
            set
            {
                userIdentificationTagContainerDTOList = value;
            }
        }
        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }
            set
            {
                phoneNumber = value;
            }
        }
    }
}
