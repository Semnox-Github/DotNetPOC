/********************************************************************************************
 * Project Name - User DTO
 * Description  - Data object of user
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        25-Jan-2016   Raghuveera          Created 
 *2.70        03-Apr-2019   Guru S A            Booking phase 2 changes 
 *2.70.2      15-Jul-2019   Girish Kundar       Modified : Added Parametrized Constructor with required fields
 *2.70.2      01-Jan-2020   Jeevan              Modification: Removed securityTokenDTO  property
 *2.90.0      09-Jul-2020   Akshay Gulaganji    Modified : Added fields DateOfBirth, ShiftConfigurationId
 *2.110.0     22-Dec-2020   Gururaja Kanjan     Modified : Added DataAccessRuleDTO
 *2.140.0     23-June-2021  Prashanth V         Modified : Added USER_ID_TAG in SearchByUserParameters
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.User
{
    /// <summary>
    ///  This is the user data object class. This acts as data holder for the user business object
    /// </summary>    
    public class UsersDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByUserParameters
        {
            /// <summary>
            /// Search by USER_ID field
            /// </summary>
            USER_ID,
            /// <summary>
            /// Search by USER_NAME field
            /// </summary>
            USER_NAME,
            /// <summary>
            /// Search by LOGIN_ID field
            /// </summary>
            LOGIN_ID,
            /// <summary>
            /// Search by EMAIL field
            /// </summary>
            EMAIL,
            /// <summary>
            /// Search by USER_STATUS field
            /// </summary>
            USER_STATUS,
            /// <summary>
            /// Search by CARD_NUMBER field
            /// </summary>
            CARD_NUMBER,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by LAST_UPDATE_DATE field
            /// </summary>
            LAST_UPDATE_DATE,
            /// <summary>//starts:Modification  on 28-Jun-2016 added site id
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,//Ends:Modification  on 28-Jun-2016 added site id
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>//starts:Modification on 28-Oct-2016 added emp number
            /// Search by EMP_NUMBER field
            /// </summary>
            EMP_NUMBER, //Ends:Modification  on 28-Oct-2016 added emp number
            /// <summary>//starts:Modification on 22-Mar-2017 added emp RoleId
            /// Search by ROle Id field
            /// </summary>RoleId
            ROLE_ID, //Ends:Modification  on 22-Mar-2017added emp RoleId
            ///<summary>
            ///Search by DepartmentId Id field
            ///</summary>
            DEPARTMENT_ID,
            ///<summary>
            ///Search by ROLE_NOT_IN field
            ///</summary>
            ROLE_NOT_IN,
            /// <summary>
            /// Search by Role Id field
            /// </summary>RoleId
            ROLE_ID_LIST,
            ///<summary>
            ///Search by SHIFT CONFIGURATION ID field
            ///</summary>
            SHIFT_CONFIGURATION_ID,
            ///<summary>
            ///Search by USER_ID_LIST field
            ///</summary>
            USER_ID_LIST,
            ///<summary>
            ///Search by USER_FIRST_NAME field
            ///</summary>
            USER_FIRST_OR_LAST_NAME,
            ///<summary>
            /// Search by USER_ID_TAG
            ///</summary>
            USER_ID_TAG
        }

        private int userId;
        private string userName;
        private string loginId;
        private int roleId;
        private string cardNumber;
        private DateTime lastLoginTime;
        private DateTime logoutTime;
        private string overrideFingerPrint;
        private int posTypeId;
        private int departmentId;
        private string departmentName;
        private DateTime empStartDate;
        private DateTime empEndDate;
        private string empEndReason;
        private int managerId;
        private string empLastName;
        private string empNumber;
        private string companyAdministrator;
        private int fingerNumber;
        private string userStatus;
        private DateTime passwordChangeDate;
        private int invalidAccessAttempts;
        private DateTime lockedOutTime;
        private bool passwordChangeOnNextLogin;
        private byte[] passwordHash;
        private string passwordSalt;
        private string password;
        private int masterEntityId;
        private string email;
        private string isActive;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private bool synchStatus;
        private DateTime? dateOfBirth;
        private string phoneNumber;
        private int shiftConfigurationId;
        private List<UserIdentificationTagsDTO> userIdentificationTagsDTOList;
        private List<UserPasswordHistoryDTO> userPasswordHistoryDTOList;
		private DataAccessRuleDTO dataAccessRuleDTO;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UsersDTO()
        {
            log.LogMethodEntry();
            userId = -1;
            roleId = -1;
            managerId = -1;
            departmentId = -1;
            posTypeId = -1;
            masterEntityId = -1;
            siteId = -1;
            shiftConfigurationId = -1;
            userIdentificationTagsDTOList = new List<UserIdentificationTagsDTO>();
            userPasswordHistoryDTOList = new List<UserPasswordHistoryDTO>();
			dataAccessRuleDTO = new DataAccessRuleDTO();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required  data fields
        /// </summary>
        public UsersDTO(int userId, string userName, string loginId, int roleId, string cardNumber, DateTime lastLoginTime, DateTime logoutTime,
                        string overrideFingerPrint, int posTypeId, int departmentId, string departmentName, DateTime empStartDate, DateTime empEndDate,
                        string empEndReason, int managerId, string empLastName, string empNumber, string companyAdministrator,
                        string userStatus, DateTime passwordChangeDate, int invalidAccessAttempts,
                         DateTime lockedOutTime, bool passwordChangeOnNextLogin, byte[] passwordHash, string passwordSalt, string password, string email, string isActive, DateTime? dateOfBirth, string phoneNumber, int shiftConfigurationId)
            : this()
        {
            log.LogMethodEntry(userId, userName, loginId, roleId, cardNumber, lastLoginTime, logoutTime,
                         overrideFingerPrint, posTypeId, departmentId, departmentName, empStartDate, empEndDate,
                         empEndReason, managerId, empLastName, empNumber, companyAdministrator,
                         userStatus, passwordChangeDate, invalidAccessAttempts,
                       lockedOutTime, passwordChangeOnNextLogin, "passwordHash", "passwordSalt", "password", email, isActive, "dateOfBirth", "phoneNumber", shiftConfigurationId);
            this.userId = userId;
            this.userName = userName;
            this.loginId = loginId;
            this.roleId = roleId;
            this.cardNumber = cardNumber;
            this.lastLoginTime = lastLoginTime;
            this.logoutTime = logoutTime;
            this.overrideFingerPrint = overrideFingerPrint;
            this.posTypeId = posTypeId;
            this.departmentId = departmentId;
            this.departmentName = departmentName;
            this.empStartDate = empStartDate;
            this.empEndDate = empEndDate;
            this.empEndReason = empEndReason;
            this.managerId = managerId;
            this.empLastName = empLastName;
            this.empNumber = empNumber;
            this.companyAdministrator = companyAdministrator;
            this.userStatus = userStatus;
            this.passwordChangeDate = passwordChangeDate;
            this.invalidAccessAttempts = invalidAccessAttempts;
            this.lockedOutTime = lockedOutTime;
            this.passwordChangeOnNextLogin = passwordChangeOnNextLogin;
            this.passwordHash = passwordHash;
            this.passwordSalt = passwordSalt;
            this.password = password;
            this.email = email;
            this.isActive = isActive;
            this.dateOfBirth = dateOfBirth;
            this.phoneNumber = phoneNumber;
            this.shiftConfigurationId = shiftConfigurationId;
			this.dataAccessRuleDTO = new DataAccessRuleDTO();												
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public UsersDTO(int userId, string userName, string loginId, int roleId, string cardNumber, DateTime lastLoginTime, DateTime logoutTime,
                       string overrideFingerPrint, int posTypeId, int departmentId, string departmentName, DateTime empStartDate, DateTime empEndDate,
                       string empEndReason, int managerId, string empLastName, string empNumber, string companyAdministrator,
                       string userStatus, DateTime passwordChangeDate, int invalidAccessAttempts,
                       DateTime lockedOutTime, bool passwordChangeOnNextLogin, byte[] passwordHash, string passwordSalt,//starts:Modification  on 28-Jun-2016 Passwordhash, passwordsalt,password and masterEntityId
                       string password, int masterEntityId, string email, string isActive, string guid,//Ends:Modification  on 28-Jun-2016 Passwordhash, passwordsalt,password and masterEntityId
                        string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, int siteId, bool synchStatus, DateTime? dateOfBirth, string phoneNumber, int shiftConfigurationId)
           : this(userId, userName, loginId, roleId, cardNumber, lastLoginTime, logoutTime,
                        overrideFingerPrint, posTypeId, departmentId, departmentName, empStartDate, empEndDate,
                        empEndReason, managerId, empLastName, empNumber, companyAdministrator,
                        userStatus, passwordChangeDate, invalidAccessAttempts,
                       lockedOutTime, passwordChangeOnNextLogin, passwordHash, passwordSalt, password, email, isActive, dateOfBirth, phoneNumber, shiftConfigurationId)
        {
            log.LogMethodEntry(userId, userName, loginId, roleId, cardNumber, lastLoginTime, logoutTime,
                         overrideFingerPrint, posTypeId, departmentId, departmentName, empStartDate, empEndDate,
                         empEndReason, managerId, empLastName, empNumber, companyAdministrator,
                         userStatus, passwordChangeDate, invalidAccessAttempts,
                         lockedOutTime, passwordChangeOnNextLogin, "passwordHash", "passwordSalt", "password",
                         masterEntityId, email, isActive, guid, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate,
                            siteId, synchStatus, "dateOfBirth", "phoneNumber", shiftConfigurationId);

            this.masterEntityId = masterEntityId;
            this.guid = guid;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required  data fields
        /// </summary>
        public UsersDTO(UsersDTO usersDTO)
        {
            log.LogMethodEntry(usersDTO);
            userId = usersDTO.userId;
            userName = usersDTO.userName;
            loginId = usersDTO.loginId;
            roleId = usersDTO.roleId;
            cardNumber = usersDTO.cardNumber;
            lastLoginTime = usersDTO.lastLoginTime;
            logoutTime = usersDTO.logoutTime;
            overrideFingerPrint = usersDTO.overrideFingerPrint;
            posTypeId = usersDTO.posTypeId;
            departmentId = usersDTO.departmentId;
            departmentName = usersDTO.departmentName;
            empStartDate = usersDTO.empStartDate;
            empEndDate = usersDTO.empEndDate;
            empEndReason = usersDTO.empEndReason;
            managerId = usersDTO.managerId;
            empLastName = usersDTO.empLastName;
            empNumber = usersDTO.empNumber;
            companyAdministrator = usersDTO.companyAdministrator;
            userStatus = usersDTO.userStatus;
            passwordChangeDate = usersDTO.passwordChangeDate;
            invalidAccessAttempts = usersDTO.invalidAccessAttempts;
            lockedOutTime = usersDTO.lockedOutTime;
            passwordChangeOnNextLogin = usersDTO.passwordChangeOnNextLogin;
            passwordHash = usersDTO.passwordHash;
            passwordSalt = usersDTO.passwordSalt;
            password = usersDTO.password;
            email = usersDTO.email;
            isActive = usersDTO.isActive;
            dateOfBirth = usersDTO.dateOfBirth;
            phoneNumber = usersDTO.phoneNumber;
            shiftConfigurationId = usersDTO.shiftConfigurationId;
            masterEntityId = usersDTO.masterEntityId;
            guid = usersDTO.guid;
            createdBy = usersDTO.createdBy;
            creationDate = usersDTO.creationDate;
            lastUpdatedBy = usersDTO.lastUpdatedBy;
            lastUpdatedDate = usersDTO.lastUpdatedDate;
            siteId = usersDTO.siteId;
            synchStatus = usersDTO.synchStatus;
            if(usersDTO.userIdentificationTagsDTOList != null)
            {
                userIdentificationTagsDTOList = new List<UserIdentificationTagsDTO>();
                foreach(var userIdentificationTagsDTO in usersDTO.userIdentificationTagsDTOList)
                {
                    userIdentificationTagsDTOList.Add(new UserIdentificationTagsDTO(userIdentificationTagsDTO));
                }
            }
			dataAccessRuleDTO = usersDTO.dataAccessRuleDTO;
			log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        [DisplayName("User ID")]
        [ReadOnly(true)]
        public int UserId { get { return userId; } set { userId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UserName field
        /// </summary>
        [DisplayName("User Name")]
        public string UserName { get { return userName; } set { userName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LoginId field
        /// </summary>
        [DisplayName("Login Id")]
        [ReadOnly(true)]
        public string LoginId { get { return loginId; } set { loginId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RoleId field
        /// </summary>
        [DisplayName("Role")]
        public int RoleId { get { return roleId; } set { roleId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        [DisplayName("Card Number")]
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastLoginTime field
        /// </summary>
        [DisplayName("Last Login Time")]
        [ReadOnly(true)]
        public DateTime LastLoginTime { get { return lastLoginTime; } set { lastLoginTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LogoutTime field
        /// </summary>
        [DisplayName("Logout Time")]
        [ReadOnly(true)]
        public DateTime LogoutTime { get { return logoutTime; } set { logoutTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the OverrideFingerPrint field
        /// </summary>
        [DisplayName("Override Finger Print?")]
        public string OverrideFingerPrint { get { return overrideFingerPrint; } set { overrideFingerPrint = value; this.IsChanged = true; } }
        /// <summary>//starts:Modification  on 28-Jun-2016 Passwordhash, passwordsalt,password and masterEntityId
        /// Get/Set method of the Password Hash field
        /// </summary>
        [DisplayName("Password Hash")]
        public byte[] PasswordHash { get { return passwordHash; } set { passwordHash = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Password Salt field
        /// </summary>
        [DisplayName("Password Salt")]
        public string PasswordSalt { get { return passwordSalt; } set { passwordSalt = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Password field
        /// </summary>
        [DisplayName("Password")]
        public string Password { get { return password; } set { password = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }//Ends:Modification  on 28-Jun-2016 Passwordhash, passwordsalt,password and masterEntityId
        /// <summary>
        /// Get/Set method of the PosTypeId field
        /// </summary>
        [DisplayName("Pos Type")]
        public int PosTypeId { get { return posTypeId; } set { posTypeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DepartmentId field
        /// </summary>
        [DisplayName("Department")]
        public int DepartmentId { get { return departmentId; } set { departmentId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DepartmentName field
        /// </summary>
        [DisplayName("Department Name")]
        public string DepartmentName { get { return departmentName; } set { departmentName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the EmpStartDate field
        /// </summary>
        [DisplayName("Employ Start Date")]
        public DateTime EmpStartDate { get { return empStartDate; } set { empStartDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the EmpEndDate field
        /// </summary>
        [DisplayName("Employ End Date")]
        public DateTime EmpEndDate { get { return empEndDate; } set { empEndDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the EmpEndReason field
        /// </summary>
        [DisplayName("Employ End Reason")]
        public string EmpEndReason { get { return empEndReason; } set { empEndReason = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ManagerId field
        /// </summary>
        [DisplayName("Manager")]
        public int ManagerId { get { return managerId; } set { managerId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Last Name field
        /// </summary>
        [DisplayName("Last Name")]
        public string EmpLastName { get { return empLastName; } set { empLastName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        [DisplayName("Employ Number")]
        public string EmpNumber { get { return empNumber; } set { empNumber = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CompanyAdministrator field
        /// </summary>
        [DisplayName("Company Administrator")]
        public string CompanyAdministrator { get { return companyAdministrator; } set { companyAdministrator = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the FingerNumber field
        /// </summary>
        [DisplayName("Finger Number")]
        public int FingerNumber { get { return fingerNumber; } set { fingerNumber = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UserStatus field
        /// </summary>
        [DisplayName("User Status")]
        public string UserStatus { get { return userStatus; } set { userStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PasswordChangeDate field
        /// </summary>
        [DisplayName("Password Change Date")]
        public DateTime PasswordChangeDate { get { return passwordChangeDate; } set { passwordChangeDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the InvalidAccessAttempts field
        /// </summary>
        [DisplayName("Invalid Access Attempts")]
        public int InvalidAccessAttempts { get { return invalidAccessAttempts; } set { invalidAccessAttempts = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LockedOutTime field
        /// </summary>
        [DisplayName("Locked Out Time")]
        public DateTime LockedOutTime { get { return lockedOutTime; } set { lockedOutTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PasswordChangeOnNextLogin field
        /// </summary>
        [DisplayName("Password Change On Next Login?")]
        public bool PasswordChangeOnNextLogin { get { return passwordChangeOnNextLogin; } set { passwordChangeOnNextLogin = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Email field
        /// </summary>
        [DisplayName("Email")]
        public string Email { get { return email; } set { email = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive == "Y" ? true : false; } set { isActive = (value == true) ? "Y" : "N"; this.IsChanged = true; } }
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
        /// Get/Set method of the UpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }
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
        /// Get/Set method of the DateOfBirth field
        /// </summary>
        [DisplayName("Date Of Birth")]
        [Browsable(true)]
        public DateTime? DateOfBirth { get { return dateOfBirth; } set { dateOfBirth = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PhoneNumber field
        /// </summary>
        [DisplayName("Phone Number")]
        [Browsable(true)]
        public string PhoneNumber { get { return phoneNumber; } set { phoneNumber = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ShiftConfigurationId field
        /// </summary>
        [DisplayName("Shift Configuration Id")]
        [Browsable(true)]
        public int ShiftConfigurationId { get { return shiftConfigurationId; } set { shiftConfigurationId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set methods for UserIdentificationTagsDTOList
        /// </summary>
        public List<UserIdentificationTagsDTO> UserIdentificationTagsDTOList
        {
            get
            {
                return userIdentificationTagsDTOList;
            }

            set
            {
                userIdentificationTagsDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set methods for userPasswordHistoryDTOList
        /// </summary>
        public List<UserPasswordHistoryDTO> UserPasswordHistoryDTOList
        {
            get
            {
                return userPasswordHistoryDTOList;
            }

            set
            {
                userPasswordHistoryDTOList = value;
            }
        }

        /// <summary>
        /// Returns whether the UserDTO changed or any of its UserIdentificationTagsDTO children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (userIdentificationTagsDTOList != null &&
                   userIdentificationTagsDTOList.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || userId < 0;
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
		public void setdataAccessRuleDTO(DataAccessRuleDTO dataAccessRuleDTO)
        {
            this.dataAccessRuleDTO = dataAccessRuleDTO;
        }

        public DataAccessRuleDTO getdataAccessRuleDTO()
        {
            return this.dataAccessRuleDTO;
        }


        public DataAccessRuleDTO DataAccessRuleDTO
        {
            get
            {
                return dataAccessRuleDTO;
            }

            set
            {
                dataAccessRuleDTO = value;
            }
        }
 
    }
}
