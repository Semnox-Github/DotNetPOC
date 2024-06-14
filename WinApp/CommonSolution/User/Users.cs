/********************************************************************************************
 * Project Name - Users
 * Description  - Bussiness logic of Users
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        27-Jan-2016   Raghuveera      Created 
 *1.01        30-Jun-2016   Jeevan          Modified -  Added Method  Users(string  LoginId)
 *2.70        11-Mar-2019   Jagan Mohan     Created -  SaveUpdateUsersList()
              08-May-2019   Mushahid Faizan Modified - SaveUpdateUsersList()
 *2.70.3      15-Jul-2019   Girish Kundar   Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.80.0      05-Jun-2020   Girish Kundar   Modified : Password change logic implemented for WMS 
 *2.80.0      26-Feb-2020   Indrajeet K     Modified : Save() method and GetAllUser() to support - Load Child UserIdentificationTag.
 *2.100.0     05-Sep-2020   Girish Kundar   Modified : POS UI redesign related changes 
 *2.110.0     27-Nov-2020   Lakshminarayana Modified : Changed as part of POS UI redesign. Implemented the new design principles
 *2.110.0     22-Dec-2020   Gururaja 		Modified : Changed as part of AFM Service Request update for populating DataAccessRuleDTO
 *2.110.0     13-Jan-2021   Deeksha 		Modified : Changed as part of Attendance Pay Rate enhancement to validate shift tracking.
 *2.130.0     14-Jun-2021   Deeksha 		Modified : Changed as part of Attendance Pay Rate enhancement to support attendance modifications.
 *2.140.0     14-Sep-2021   Deeksha 		Modified : Provisional Shift changes
 *2.140.0     14-Sep-2021   Girish Kundar   Modified : Added GetCurrentClockedInUsers() method to get currently logged in user
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Users allows to access the users details based on the business logic.
    /// </summary>
    public class Users
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private UsersDTO usersDTO;
        private readonly string passPhrase;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private Users(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.passPhrase = GetUsersPassPhrase();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parameterUsersDTO">usersDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public Users(ExecutionContext executionContext, UsersDTO parameterUsersDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterUsersDTO, sqlTransaction);

            if (parameterUsersDTO.UserId > -1)
            {
                LoadUserDTO(parameterUsersDTO.UserId, true, true, sqlTransaction);//added sql
                ThrowIfUserDTOIsNull(parameterUsersDTO.UserId);
                Update(parameterUsersDTO);
            }
            else
            {
                ValidateUserName(parameterUsersDTO.UserName);
                ValidateLoginId(parameterUsersDTO.LoginId);
                ValidateUserStatus(parameterUsersDTO.UserStatus);
                ValidateRoleId(parameterUsersDTO.RoleId);
                ValidatePassword(parameterUsersDTO.Password, parameterUsersDTO.RoleId, parameterUsersDTO.UserId, parameterUsersDTO.LoginId);
                ValidateEmployeeNumber(parameterUsersDTO.EmpNumber);
                ValidateEmpStartDateAndEndDate(parameterUsersDTO.EmpStartDate, parameterUsersDTO.EmpEndDate);
                ValidateManagerId(parameterUsersDTO.UserId, parameterUsersDTO.ManagerId);
                string passwordSalt = new RandomString(10);
                byte[] passwordHash = new UserPasswordHash(parameterUsersDTO.Password, passwordSalt, new UserEncryptionKey(executionContext, parameterUsersDTO.LoginId.ToLower()));
                usersDTO = new UsersDTO(-1, parameterUsersDTO.UserName, parameterUsersDTO.LoginId, parameterUsersDTO.RoleId, string.Empty, DateTime.MinValue, DateTime.MinValue, parameterUsersDTO.OverrideFingerPrint,
                                              parameterUsersDTO.PosTypeId, parameterUsersDTO.DepartmentId, string.Empty, parameterUsersDTO.EmpStartDate, parameterUsersDTO.EmpEndDate, parameterUsersDTO.EmpEndReason,
                                              parameterUsersDTO.ManagerId, parameterUsersDTO.EmpLastName, parameterUsersDTO.EmpNumber, parameterUsersDTO.CompanyAdministrator, parameterUsersDTO.UserStatus, ServerDateTime.Now,
                                              0, DateTime.MinValue, parameterUsersDTO.PasswordChangeOnNextLogin, passwordHash, passwordSalt, string.Empty, parameterUsersDTO.Email, parameterUsersDTO.IsActive ? "Y" : "N", parameterUsersDTO.DateOfBirth, parameterUsersDTO.PhoneNumber, parameterUsersDTO.ShiftConfigurationId);
                if (parameterUsersDTO.UserIdentificationTagsDTOList != null && parameterUsersDTO.UserIdentificationTagsDTOList.Any())
                {
                    usersDTO.UserIdentificationTagsDTOList = new List<UserIdentificationTagsDTO>();
                    foreach (UserIdentificationTagsDTO parameterUserIdentificationTagsDTO in parameterUsersDTO.UserIdentificationTagsDTOList)
                    {
                        if (parameterUserIdentificationTagsDTO.Id > -1)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 2196, "UserIdentificationTag", parameterUserIdentificationTagsDTO.Id);
                            log.LogMethodExit(null, "Throwing Exception - " + message);
                            throw new EntityNotFoundException(message);
                        }
                        var userIdentificationTagsDTO = new UserIdentificationTagsDTO(-1, -1, parameterUserIdentificationTagsDTO.CardNumber,
                                                           parameterUserIdentificationTagsDTO.FingerPrint, parameterUserIdentificationTagsDTO.FingerNumber, parameterUserIdentificationTagsDTO.ActiveFlag,
                                                           parameterUserIdentificationTagsDTO.StartDate, parameterUserIdentificationTagsDTO.EndDate, parameterUserIdentificationTagsDTO.AttendanceReaderTag,
                                                           parameterUserIdentificationTagsDTO.CardId, parameterUserIdentificationTagsDTO.FPTemplate, string.Empty);
                        UserIdentificationTags userIdentificationTags = new UserIdentificationTags(executionContext, userIdentificationTagsDTO);
                        usersDTO.UserIdentificationTagsDTOList.Add(userIdentificationTags.UserIdentificationTagsDTO);
                    }
                }

            }
            log.LogMethodExit();
        }

        public void AddStaffCard(StaffCardDTO staffCardDTO, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(staffCardDTO);
            if (!string.IsNullOrEmpty(staffCardDTO.CardNumber))
            {
                if (usersDTO.UserIdentificationTagsDTOList.Any())
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1160));
                }
                DateTime currentTime = ServerDateTime.Now;
                UserIdentificationTagsDTO userIdTagDTO = new UserIdentificationTagsDTO(-1, usersDTO.UserId, staffCardDTO.CardNumber, "", -1, true,
                    currentTime, DateTime.MinValue, executionContext.GetUserId(), currentTime, "", executionContext.GetSiteId(), false, -1, true, staffCardDTO.CardId, executionContext.GetUserId(), currentTime, null, null);
                HRApprovalLogsDTO hrApprovalLogsDTO = new HRApprovalLogsDTO(-1, Convert.ToString(HRApprovalLogsDTO.EntityType.USERIDENTIFICATIONTAGS), userIdTagDTO.Guid, Convert.ToString(HRApprovalLogsDTO.ActionType.ADD),
                staffCardDTO.ManagerId.ToString(), null, "", staffCardDTO.Remarks, "",
                null, "", null, executionContext.GetMachineId(), executionContext.GetSiteId(), "", false, -1);
                userIdTagDTO.HRApprovalLogsDTOList.Add(hrApprovalLogsDTO);
                usersDTO.UserIdentificationTagsDTOList.Add(userIdTagDTO);
                Save(sqlTrx);
            }
            log.LogMethodExit();
        }
        public void RemoveStaffCard(StaffCardDTO staffCardDTO, SqlTransaction sql)
        {
            log.LogMethodEntry(staffCardDTO);
            if (usersDTO.UserIdentificationTagsDTOList != null && usersDTO.UserIdentificationTagsDTOList.Any())
            {
                UserIdentificationTagsDTO userIdentificationTagsDTO = usersDTO.UserIdentificationTagsDTOList.FirstOrDefault(t => t.CardNumber.Equals(staffCardDTO.CardNumber));
                if (userIdentificationTagsDTO != null)
                {
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    HRApprovalLogsDTO hrApprovalLogsDTO = new HRApprovalLogsDTO(-1, Convert.ToString(HRApprovalLogsDTO.EntityType.USERIDENTIFICATIONTAGS), userIdentificationTagsDTO.Guid, Convert.ToString(HRApprovalLogsDTO.ActionType.DEACTIVATE),
                                    staffCardDTO.ManagerId.ToString(), null, "", "", "",
                                    null, "", null, executionContext.GetMachineId(), executionContext.GetSiteId(), "", false, -1);
                    userIdentificationTagsDTO.HRApprovalLogsDTOList.Add(hrApprovalLogsDTO);
                    userIdentificationTagsDTO.ActiveFlag = false;
                    userIdentificationTagsDTO.EndDate = lookupValuesList.GetServerDateTime();
                    UserIdentificationTags userIdentificationTagsBL = new UserIdentificationTags(executionContext, userIdentificationTagsDTO);
                    userIdentificationTagsBL.Save(sql);
                }
            }
            log.LogMethodExit();
        }

        private void LoadUserDTO(int userId, bool loadChildRecords, bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(userId, loadChildRecords, activeChildRecords);
            UsersDataHandler usersDataHandler = new UsersDataHandler(passPhrase, sqlTransaction);
            usersDTO = usersDataHandler.GetUsers(userId);
            ThrowIfUserDTOIsNull(userId);
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void ThrowIfUserDTOIsNull(int userId)
        {
            log.LogMethodEntry(userId);
            if (usersDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "users", userId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            UserIdentificationTagListBL userIdentificationTagListBL = new UserIdentificationTagListBL();
            List<UserIdentificationTagsDTO> userIdentificationTagsDTOList = userIdentificationTagListBL.GetUserIdentificationTagDTOListOfUsers(new List<int>() { usersDTO.UserId }, activeChildRecords, sqlTransaction);
            usersDTO.UserIdentificationTagsDTOList = userIdentificationTagsDTOList;
            DataAccessRuleDTO dataAccessRuleDTO = BuildDataAccessRuleDTO(usersDTO.RoleId, executionContext);
            usersDTO.DataAccessRuleDTO = dataAccessRuleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the userId parameter
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="loadChildRecords">To load the child DTO Records</param>
        public Users(ExecutionContext executionContext, int userId, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, userId, loadChildRecords, activeChildRecords, sqlTransaction);
            LoadUserDTO(userId, loadChildRecords, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with loginId and siteId. Creates the user of the given login Id and site Id
        /// </summary>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <param name="sqlTransaction"></param>
        public Users(ExecutionContext executionContext, string loginId, int siteId = -1, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        : this(executionContext)
        {
            log.LogMethodEntry(loginId, siteId, sqlTransaction);
            if (string.IsNullOrWhiteSpace(loginId))
            {
                string errorMessage = MessageContainerList.GetMessage(siteId, 1144, MessageContainerList.GetMessage(siteId, "Login Id"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage);
            }
            UsersDataHandler usersDataHandler = new UsersDataHandler(passPhrase, sqlTransaction);
            usersDTO = usersDataHandler.GetUsers(loginId, siteId);
            if (usersDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "users", "LoginId :" + loginId + " SiteId :" + siteId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }

            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get UserDto Object
        /// </summary>
        public UsersDTO UserDTO
        {
            get
            {
                UsersDTO result = new UsersDTO(usersDTO);
                result.Password = string.Empty;
                result.PasswordHash = null;
                result.PasswordSalt = string.Empty;
                result.AcceptChanges();
                return result;
            }
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SaveImpl(sqlTransaction, true);
            if (!string.IsNullOrEmpty(usersDTO.Guid))
            {
                AuditLog auditLog = new AuditLog(executionContext);
                auditLog.AuditTable("users", usersDTO.Guid, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Users
        /// Checks if the User id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        private void SaveImpl(SqlTransaction sqlTransaction, bool updateWhoColumns)
        {
            log.LogMethodEntry(sqlTransaction, updateWhoColumns);

            UsersDataHandler usersDataHandler = new UsersDataHandler(Users.GetUsersPassPhrase(), sqlTransaction);
            if (usersDTO.UserId < 0)
            {
                usersDTO = usersDataHandler.InsertUser(usersDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                usersDTO.AcceptChanges();
            }
            else
            {
                if (usersDTO.IsChanged)
                {
                    if (updateWhoColumns)
                    {
                        usersDTO = usersDataHandler.UpdateUser(usersDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    }
                    else
                    {
                        usersDTO = usersDataHandler.UpdateUser(usersDTO);
                    }
                    usersDTO.AcceptChanges();
                }
                log.LogMethodExit();
            }
            // Will Save the Child UserIdentificationTagsDTO
            log.Debug("usersDTO.UserIdentificationTagsDTO Value :" + usersDTO.UserIdentificationTagsDTOList);
            if (usersDTO.UserIdentificationTagsDTOList != null && usersDTO.UserIdentificationTagsDTOList.Any())
            {
                List<UserIdentificationTagsDTO> updatedUserIdentificationTagsDTO = new List<UserIdentificationTagsDTO>();
                foreach (UserIdentificationTagsDTO UserIdentificationTagsDTO in usersDTO.UserIdentificationTagsDTOList)
                {
                    if (UserIdentificationTagsDTO.UserId != usersDTO.UserId)
                    {
                        UserIdentificationTagsDTO.UserId = usersDTO.UserId;
                    }
                    log.Debug("UserIdentificationTagsDTO.IsChanged Value :" + UserIdentificationTagsDTO.IsChanged);
                    if (UserIdentificationTagsDTO.IsChanged)
                    {
                        updatedUserIdentificationTagsDTO.Add(UserIdentificationTagsDTO);
                    }
                }
                log.Debug("updatedUserIdentificationTagsDTO Value :" + updatedUserIdentificationTagsDTO);
                if (updatedUserIdentificationTagsDTO.Any())
                {
                    UserIdentificationTagListBL userIdTagsListBL = new UserIdentificationTagListBL(executionContext, updatedUserIdentificationTagsDTO);
                    userIdTagsListBL.Save(sqlTransaction);
                }
            }

            // Will Save the Child UserPasswordHistoryDTO
            log.Debug("usersDTO.UserPasswordHistoryDTO Value :" + usersDTO.UserPasswordHistoryDTOList);
            if (usersDTO.UserPasswordHistoryDTOList != null && usersDTO.UserPasswordHistoryDTOList.Any())
            {
                List<UserPasswordHistoryDTO> updatedUserPasswordHistoryDTOList = new List<UserPasswordHistoryDTO>();
                foreach (UserPasswordHistoryDTO userPasswordHistoryDTO in usersDTO.UserPasswordHistoryDTOList)
                {
                    if (userPasswordHistoryDTO.UserId != usersDTO.UserId)
                    {
                        userPasswordHistoryDTO.UserId = usersDTO.UserId;
                    }
                    log.Debug("UserPasswordHistoryDTO.IsChanged Value :" + userPasswordHistoryDTO.IsChanged);
                    if (userPasswordHistoryDTO.IsChanged)
                    {
                        updatedUserPasswordHistoryDTOList.Add(userPasswordHistoryDTO);
                    }
                }
                log.Debug("updatedUserPasswordHistoryDTO Value :" + updatedUserPasswordHistoryDTOList);
                if (updatedUserPasswordHistoryDTOList.Any())
                {
                    UserPasswordHistoryListBL userPasswordHistoryListBL = new UserPasswordHistoryListBL(executionContext, updatedUserPasswordHistoryDTOList);
                    userPasswordHistoryListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        private void Update(UsersDTO parameterUsersDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterUsersDTO, sqlTransaction);
            ChangeUserName(parameterUsersDTO.UserName);
            ChangeRoleId(parameterUsersDTO.RoleId);
            ChangeUserStatus(parameterUsersDTO.UserStatus);
            if (string.IsNullOrWhiteSpace(parameterUsersDTO.Password) == false)
            {
                ChangePassword(parameterUsersDTO.Password);
            }
            ChangeEmployeeNumber(parameterUsersDTO.EmpNumber);
            ChangeIsActive(parameterUsersDTO.IsActive);
            ChangePOSTypeId(parameterUsersDTO.PosTypeId);
            ChangeDepartmentId(parameterUsersDTO.DepartmentId);
            ChangeEmployeeStartDateAndEndDate(parameterUsersDTO.EmpStartDate, parameterUsersDTO.EmpEndDate);
            ChangeEmployeeEndReason(parameterUsersDTO.EmpEndReason);
            ChangeManagerId(parameterUsersDTO.ManagerId);
            ChangeEmployeeLastName(parameterUsersDTO.EmpLastName);
            ChangeCompanyAdministrator(parameterUsersDTO.CompanyAdministrator);
            ChangePasswordChangeOnNextLogin(parameterUsersDTO.PasswordChangeOnNextLogin);
            ChangeOverrideFingerPrint(parameterUsersDTO.OverrideFingerPrint);
            ChangeEmail(parameterUsersDTO.Email);
            ChangePhoneNumber(parameterUsersDTO.PhoneNumber);
            ChangeDateOfBirth(parameterUsersDTO.DateOfBirth);
            ChangeShiftConfigurationId(parameterUsersDTO.ShiftConfigurationId);
            if (usersDTO.LockedOutTime != DateTime.MinValue &&
                parameterUsersDTO.LockedOutTime == DateTime.MinValue)
            {
                Activate();
            }
            Dictionary<int, UserIdentificationTagsDTO> userIdentificationTagsDTODictionary = new Dictionary<int, UserIdentificationTagsDTO>();
            if (usersDTO.UserIdentificationTagsDTOList != null &&
                usersDTO.UserIdentificationTagsDTOList.Any())
            {
                foreach (var userIdentificationTagsDTO in usersDTO.UserIdentificationTagsDTOList)
                {
                    userIdentificationTagsDTODictionary.Add(userIdentificationTagsDTO.Id, userIdentificationTagsDTO);
                }
            }
            if (parameterUsersDTO.UserIdentificationTagsDTOList != null &&
                parameterUsersDTO.UserIdentificationTagsDTOList.Any())
            {
                foreach (var parameterUserIdentificationTagsDTO in parameterUsersDTO.UserIdentificationTagsDTOList)
                {
                    if (userIdentificationTagsDTODictionary.ContainsKey(parameterUserIdentificationTagsDTO.Id))
                    {
                        UserIdentificationTags userIdentificationTags = new UserIdentificationTags(executionContext, userIdentificationTagsDTODictionary[parameterUserIdentificationTagsDTO.Id]);
                        userIdentificationTags.Update(parameterUserIdentificationTagsDTO);
                    }
                    else if (parameterUserIdentificationTagsDTO.Id > -1)
                    {
                        UserIdentificationTags userIdentificationTags = new UserIdentificationTags(executionContext, parameterUserIdentificationTagsDTO.Id, sqlTransaction);
                        if (usersDTO.UserIdentificationTagsDTOList == null)
                        {
                            usersDTO.UserIdentificationTagsDTOList = new List<UserIdentificationTagsDTO>();
                        }
                        usersDTO.UserIdentificationTagsDTOList.Add(userIdentificationTags.UserIdentificationTagsDTO);
                        userIdentificationTags.Update(parameterUserIdentificationTagsDTO);
                    }
                    else
                    {
                        UserIdentificationTags userIdentificationTags = new UserIdentificationTags(executionContext, parameterUserIdentificationTagsDTO);
                        if (usersDTO.UserIdentificationTagsDTOList == null)
                        {
                            usersDTO.UserIdentificationTagsDTOList = new List<UserIdentificationTagsDTO>();
                        }
                        usersDTO.UserIdentificationTagsDTOList.Add(userIdentificationTags.UserIdentificationTagsDTO);
                    }
                }
            }

            log.LogMethodExit();
        }

        public void ChangeShiftConfigurationId(int shiftConfigurationId)
        {
            log.LogMethodEntry(shiftConfigurationId);
            if (usersDTO.ShiftConfigurationId == shiftConfigurationId)
            {
                log.LogMethodExit(null, "No changes to user shiftConfigurationId");
                return;
            }
            usersDTO.ShiftConfigurationId = shiftConfigurationId;
            log.LogMethodExit();
        }

        public void ChangePhoneNumber(string phoneNumber)
        {
            log.LogMethodEntry(phoneNumber);
            if (usersDTO.PhoneNumber == phoneNumber)
            {
                log.LogMethodExit(null, "No changes to user phoneNumber");
                return;
            }
            usersDTO.PhoneNumber = phoneNumber;
            log.LogMethodExit();
        }

        public void ChangeDateOfBirth(DateTime? dateOfBirth)
        {
            log.LogMethodEntry(dateOfBirth);
            if (usersDTO.DateOfBirth == dateOfBirth)
            {
                log.LogMethodExit(null, "No changes to user dateOfBirth");
                return;
            }
            usersDTO.DateOfBirth = dateOfBirth;
            log.LogMethodExit();
        }

        public void ChangeEmail(string email)
        {
            log.LogMethodEntry(email);
            if (usersDTO.Email == email)
            {
                log.LogMethodExit(null, "No changes to user email");
                return;
            }
            usersDTO.Email = email;
            log.LogMethodExit();
        }

        public void ChangeOverrideFingerPrint(string overrideFingerPrint)
        {
            log.LogMethodEntry(overrideFingerPrint);
            if (usersDTO.OverrideFingerPrint == overrideFingerPrint)
            {
                log.LogMethodExit(null, "No changes to user overrideFingerPrint");
                return;
            }
            usersDTO.OverrideFingerPrint = overrideFingerPrint;
            log.LogMethodExit();
        }

        public void ChangePasswordChangeOnNextLogin(bool passwordChangeOnNextLogin)
        {
            log.LogMethodEntry(passwordChangeOnNextLogin);
            if (usersDTO.PasswordChangeOnNextLogin == passwordChangeOnNextLogin)
            {
                log.LogMethodExit(null, "No changes to user passwordChangeOnNextLogin");
                return;
            }
            usersDTO.PasswordChangeOnNextLogin = passwordChangeOnNextLogin;
            log.LogMethodExit();
        }

        public void ChangeCompanyAdministrator(string companyAdministrator)
        {
            log.LogMethodEntry(companyAdministrator);
            if (usersDTO.CompanyAdministrator == companyAdministrator)
            {
                log.LogMethodExit(null, "No changes to user companyAdministrator");
                return;
            }
            usersDTO.CompanyAdministrator = companyAdministrator;
            log.LogMethodExit();
        }

        public void ChangeEmployeeLastName(string empLastName)
        {
            log.LogMethodEntry(empLastName);
            if (usersDTO.EmpLastName == empLastName)
            {
                log.LogMethodExit(null, "No changes to user empLastName");
                return;
            }
            usersDTO.EmpLastName = empLastName;
            log.LogMethodExit();
        }

        public void ChangeManagerId(int managerId)
        {
            log.LogMethodEntry(managerId);
            if (usersDTO.ManagerId == managerId)
            {
                log.LogMethodExit(null, "No changes to user managerId");
                return;
            }
            ValidateManagerId(UserDTO.UserId, managerId);
            usersDTO.ManagerId = managerId;
            log.LogMethodExit();
        }

        private void ValidateManagerId(int userId, int managerId)
        {
            log.LogMethodEntry(managerId);
            if (managerId == -1 || userId != managerId)
            {
                log.LogMethodExit(null, "managerId == -1 || userId != managerId");
                return;
            }

            UsersDataHandler usersDataHandler = new UsersDataHandler(passPhrase);
            bool userWithSameManagerIdExists = usersDataHandler.IsUserWithSameManagerIdExists(userId);
            if (userWithSameManagerIdExists && userId == managerId)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 627);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Manager cannot be same as User.", "User", "ManagerId", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ChangeEmployeeEndReason(string empEndReason)
        {
            log.LogMethodEntry(empEndReason);
            if (usersDTO.EmpEndReason == empEndReason)
            {
                log.LogMethodExit(null, "No changes to user empEndReason");
                return;
            }
            usersDTO.EmpEndReason = empEndReason;
            log.LogMethodExit();
        }

        private void ChangeEmployeeStartDateAndEndDate(DateTime empStartDate, DateTime empEndDate)
        {
            log.LogMethodEntry(empStartDate, empEndDate);
            if (usersDTO.EmpStartDate == empStartDate &&
                usersDTO.EmpEndDate == empEndDate)
            {
                log.LogMethodExit(null, "No changes to user empStartDate and empEndDate");
                return;
            }
            ValidateEmpStartDateAndEndDate(empStartDate, empEndDate);
            if (usersDTO.EmpStartDate != empStartDate)
            {
                usersDTO.EmpStartDate = empStartDate;
            }
            if (usersDTO.EmpEndDate != empEndDate)
            {
                usersDTO.EmpEndDate = empEndDate;
            }
            log.LogMethodExit();
        }

        private void ValidateEmpStartDateAndEndDate(DateTime empStartDate, DateTime empEndDate)
        {
            log.LogMethodEntry(empStartDate, empEndDate);
            if (empStartDate != DateTime.MinValue &&
                empEndDate != DateTime.MinValue &&
                empStartDate > empEndDate)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Employee End Date"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException(errorMessage, "User", "EmpEndDate", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ChangeDepartmentId(int departmentId)
        {
            log.LogMethodEntry(departmentId);
            if (usersDTO.DepartmentId == departmentId)
            {
                log.LogMethodExit(null, "No changes to user departmentId");
                return;
            }
            usersDTO.DepartmentId = departmentId;
            log.LogMethodExit();
        }

        public void ChangePOSTypeId(int posTypeId)
        {
            log.LogMethodEntry(posTypeId);
            if (usersDTO.PosTypeId == posTypeId)
            {
                log.LogMethodExit(null, "No changes to user posTypeId");
                return;
            }
            usersDTO.PosTypeId = posTypeId;
            log.LogMethodExit();
        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (usersDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to user isActive");
                return;
            }
            usersDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        private void ChangeEmployeeNumber(string empNumber)
        {
            log.LogMethodEntry(empNumber);

            if (usersDTO.EmpNumber == empNumber)
            {
                log.LogMethodExit(null, "No changes to user empNumber");
                return;
            }
            ValidateEmployeeNumber(empNumber);
            usersDTO.EmpNumber = empNumber;
            log.LogMethodExit();
        }

        private void ValidateEmployeeNumber(string empNumber)
        {
            log.LogMethodEntry(empNumber);
            if (string.IsNullOrWhiteSpace(empNumber))
            {
                log.LogMethodExit(null, "Empty employee number");
                return;
            }
            string afterTrim = empNumber.Trim();
            int o;
            if (int.TryParse(afterTrim, out o) == false)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 648);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Invalid Employee Number.", "User", "EmpNumber", errorMessage);
            }
            log.LogMethodExit();
        }

        public void ChangePassword(string password)
        {
            log.LogMethodEntry("password");
            Core.GenericUtilities.EventLog.LogEvent(executionContext, Core.GenericUtilities.EventLog.LogType.Security, "Changing password for " + usersDTO.LoginId, "Authentication");
            ValidatePassword(password, usersDTO.RoleId, usersDTO.UserId, usersDTO.LoginId);
            UserPasswordHistoryDTO userPasswordHistoryDTO = new UserPasswordHistoryDTO(-1, usersDTO.UserId, usersDTO.PasswordHash, ServerDateTime.Now, usersDTO.PasswordSalt);
            if (usersDTO.UserPasswordHistoryDTOList == null)
            {
                usersDTO.UserPasswordHistoryDTOList = new List<UserPasswordHistoryDTO>();
            }
            usersDTO.UserPasswordHistoryDTOList.Add(userPasswordHistoryDTO);
            usersDTO.PasswordSalt = new RandomString(10);
            usersDTO.PasswordHash = new UserPasswordHash(password, usersDTO.PasswordSalt, new UserEncryptionKey(executionContext, usersDTO.LoginId.ToLower()));
            usersDTO.PasswordChangeDate = ServerDateTime.Now;
            usersDTO.PasswordChangeOnNextLogin = false;
            usersDTO.UserStatus = UserStatus.ACTIVE.ToString();
            Core.GenericUtilities.EventLog.LogEvent(executionContext, Core.GenericUtilities.EventLog.LogType.Security, Core.GenericUtilities.EventLog.EventType.Success, "Password changed for " + usersDTO.LoginId, "Authentication");
            log.LogMethodExit();
        }

        private void ChangeUserStatus(string userStatus)
        {
            log.LogMethodEntry(userStatus);
            if (usersDTO.UserStatus == userStatus)
            {
                log.LogMethodExit(null, "No changes to user status");
                return;
            }
            ValidateUserStatus(userStatus);
            UserStatus status = (UserStatus)Enum.Parse(typeof(UserStatus), userStatus);
            switch (status)
            {
                case UserStatus.ACTIVE:
                    {
                        Activate();
                        break;
                    }
                case UserStatus.DISABLED:
                    {
                        Disable();
                        break;
                    }
                case UserStatus.LOCKED:
                    {
                        Lockout();
                        break;
                    }
                case UserStatus.INACTIVE:
                    {
                        Inactivate();
                        break;
                    }
            }
            log.LogMethodExit();
        }

        public void Inactivate()
        {
            log.LogMethodEntry();
            if (UserStatus == UserStatus.INACTIVE)
            {
                log.LogMethodExit(null, "No changes to user status");
            }
            usersDTO.UserStatus = UserStatus.INACTIVE.ToString();
            log.LogMethodExit();
        }

        public void Lockout()
        {
            log.LogMethodEntry();
            if (UserStatus == UserStatus.LOCKED)
            {
                log.LogMethodExit(null, "No changes to user status");
            }
            usersDTO.LockedOutTime = ServerDateTime.Now;
            usersDTO.UserStatus = UserStatus.LOCKED.ToString();
            log.LogMethodExit();
        }

        public void Disable()
        {
            log.LogMethodEntry();
            if (UserStatus == UserStatus.DISABLED)
            {
                log.LogMethodExit(null, "No changes to user status");
            }
            usersDTO.UserStatus = UserStatus.DISABLED.ToString();
            log.LogMethodExit();
        }

        public void Activate()
        {
            log.LogMethodEntry();
            if (UserStatus == UserStatus.ACTIVE)
            {
                log.LogMethodExit(null, "No changes to user status");
            }
            usersDTO.LockedOutTime = DateTime.MinValue;
            usersDTO.InvalidAccessAttempts = 0;
            usersDTO.UserStatus = UserStatus.ACTIVE.ToString();
            log.LogMethodExit();
        }

        private void ValidateUserStatus(string userStatus)
        {
            log.LogMethodEntry(userStatus);
            UserStatus status;
            if (Enum.TryParse<UserStatus>(userStatus, out status) == false)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2475);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("User role is empty.", "User", "UserStatus", errorMessage);
            }
            log.LogMethodExit();
            return;
        }

        private void ValidatePassword(string password, int roleId, int userId, string loginId)
        {
            log.LogMethodEntry(password);
            if (string.IsNullOrWhiteSpace(password))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2484);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("password is empty.", "User", "Password", errorMessage);
            }
            int securityPolicyId = GetSecurityPolicyId(roleId);
            int passwordMinLength = SecurityPolicyMasterList.GetPasswordMinLength(executionContext, securityPolicyId);
            int passwordMinAlphabets = SecurityPolicyMasterList.GetPasswordMinAlphabets(executionContext, securityPolicyId);
            int passwordMinNumbers = SecurityPolicyMasterList.GetPasswordMinNumbers(executionContext, securityPolicyId);
            int passwordMinSpecialChars = SecurityPolicyMasterList.GetPasswordMinSpecialChars(executionContext, securityPolicyId);
            int rememberPasswordsCount = SecurityPolicyMasterList.GetRememberPasswordsCount(executionContext, securityPolicyId);
            if (password.Length < passwordMinLength)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 814, passwordMinLength);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Minimum length criteria not met. PasswordMinLength: " + passwordMinLength, "User", "Password", errorMessage);
            }
            if (passwordMinAlphabets > 0)
            {
                if (password.Count(c => Char.IsLetter(c)) < passwordMinAlphabets)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 815, passwordMinAlphabets);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Minimum Alphabets criteria not met. PasswordMinimumAlphabets: " + passwordMinAlphabets, "User", "Password", errorMessage);
                }
            }
            if (passwordMinNumbers > 0)
            {
                if (password.Count(c => Char.IsNumber(c)) < passwordMinNumbers)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 816, passwordMinNumbers);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Minimum Numbers criteria not met. PasswordMinimumAlphabets: " + passwordMinNumbers, "User", "Password", errorMessage);
                }
            }

            if (passwordMinSpecialChars > 0)
            {
                if ((password.Length - password.Count(c => Char.IsLetter(c)) - password.Count(c => Char.IsNumber(c))) < passwordMinSpecialChars)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 816, passwordMinNumbers);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Minimum Numbers criteria not met. PasswordMinimumAlphabets: " + passwordMinSpecialChars, "User", "Password", errorMessage);
                }
            }
            if (userId > -1)
            {
                UserEncryptionKey userEncryptionKey = new UserEncryptionKey(executionContext, loginId.ToLower());
                UserPasswordHash newPasswordHash = new UserPasswordHash(password, usersDTO.PasswordSalt, userEncryptionKey);
                UserPasswordHash passwordHash = new UserPasswordHash(usersDTO.PasswordHash);
                if (newPasswordHash == passwordHash)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1920);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Password same as the previous password.", "User", "Password", errorMessage);
                }

                UserPasswordHistoryListBL userPasswordHistoryListBL = new UserPasswordHistoryListBL();
                List<KeyValuePair<UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters, string>> searchParameters = new List<KeyValuePair<UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters, string>>();
                searchParameters.Add(new KeyValuePair<UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters, string>(UserPasswordHistoryDTO.SearchByUserPasswordHistoryParameters.USER_ID, userId.ToString()));
                List<UserPasswordHistoryDTO> userPasswordHistoryDTOList = userPasswordHistoryListBL.GetUserPasswordHistoryDTOList(searchParameters);
                foreach (var userPasswordHistoryDTO in userPasswordHistoryDTOList.OrderByDescending(x => x.ChangeDate).Take(rememberPasswordsCount - 1))
                {
                    UserPasswordHistoryBL userPasswordHistoryBL = new UserPasswordHistoryBL(executionContext, userPasswordHistoryDTO);
                    if (userPasswordHistoryBL.IsMatch(password, loginId))
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 818, rememberPasswordsCount);
                        log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                        throw new ValidationException("Password history match occurred. RememberPasswordCount: " + rememberPasswordsCount, "User", "Password", errorMessage);
                    }
                }
            }

            log.LogMethodExit();
        }

        public ShiftDTO CreateNewShift(string userName, DateTime loginTime, string shiftApplication, double openingAmount, int cardCount,
                                   int shiftTicketNumber, string shiftRemarks, string loginId, double creditCardAmount, double ChequeAmount,
                                   double CouponAmount, DateTime? approvalTime, int userId, string posName, bool isRemoteOpenShift, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(userName, loginTime, shiftApplication, openingAmount, cardCount, shiftTicketNumber, shiftRemarks, loginId,
                                creditCardAmount, ChequeAmount, CouponAmount, approvalTime, userId, posName, sqlTransaction);
            ShiftBL shiftBL;
            ShiftDTO shiftDTO = null;
            string shiftLogReason;
            if (shiftApplication == "POS")
            {
                ShiftListBL shiftListBL = new ShiftListBL(executionContext);
                List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchByparams = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
                searchByparams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_USERNAME, userName));
                searchByparams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.POS_MACHINE, posName));
                searchByparams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.ORDER_BY_TIMESTAMP, "desc"));
                searchByparams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.TIMESTAMP, loginTime.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                List<ShiftDTO> shiftDTOList = shiftListBL.GetShiftDTOList(searchByparams, true);
                if (shiftDTOList != null && shiftDTOList[0].ShiftAction == ShiftDTO.ShiftActionType.Open.ToString())
                {
                    shiftDTO = shiftDTOList[0];
                    //log.Debug("Previous shift was not closed");
                    //shiftDTO.ShiftAmount = Convert.ToDouble(openingAmount);
                    //shiftDTO.ShiftRemarks = shiftRemarks;
                    //shiftDTO.ShiftTicketNumber = shiftTicketNumber.ToString();
                    //shiftDTO.CardCount = cardCount;
                    //shiftDTO.CreditCardamount = creditCardAmount;
                    //shiftDTO.ChequeAmount = ChequeAmount;
                    //shiftDTO.CouponAmount = CouponAmount;
                    shiftLogReason = "Unlock Open Shift";
                }
                else
                {
                    string shiftType = isRemoteOpenShift ? ShiftDTO.ShiftActionType.ROpen.ToString() : ShiftDTO.ShiftActionType.Open.ToString();
                    shiftLogReason = isRemoteOpenShift ? "Remote Open Shift" : "Open Shift";
                    shiftDTO = new ShiftDTO(-1, userName, loginTime, shiftApplication, shiftType, openingAmount, cardCount, shiftTicketNumber.ToString(), shiftRemarks, posName, -1, null, -1, 0,
                                                          creditCardAmount, ChequeAmount, CouponAmount, -1, -1, -1, -1, loginId, -1);
                }
            }
            else
            {
                string shiftType = isRemoteOpenShift ? ShiftDTO.ShiftActionType.ROpen.ToString() : ShiftDTO.ShiftActionType.Open.ToString();
                shiftDTO = new ShiftDTO(-1, userName, loginTime, shiftApplication, shiftType, 0, 0, shiftTicketNumber.ToString(), shiftRemarks, posName,
                                                    -1, null, -1, 0, 0, 0, 0, -1, -1, -1, -1, loginId, -1);
                shiftLogReason = "Open Shift Redemption";
            }
            shiftBL = new ShiftBL(executionContext, shiftDTO);
            shiftBL.OpenShift(sqlTransaction, shiftLogReason);
            log.LogMethodExit(shiftBL.ShiftDTO);
            return shiftBL.ShiftDTO;
        }

        public void ProvisionalClose(int shiftKey, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(shiftKey, sqlTransaction);
            ShiftBL shiftBL = new ShiftBL(executionContext, shiftKey, true, false);
            shiftBL.ProvisionalClose(sqlTransaction);
            log.LogMethodExit();
        }

        public void CloseShift(int shiftId, double actualAmount, int cardCount, int actualTickets, string remarks, decimal shiftAmount, string shiftApplication, int actualCards, decimal couponAmount,
                                decimal creditCardAmount, decimal chequeAmount, decimal actualGameCardAmount, decimal actualcreditCardAmount, decimal actualChequeAmount, decimal actualCouponAmount,
                                decimal gamecardAmount, double shiftTicketnumber, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(shiftId, actualAmount, cardCount, actualTickets, remarks, shiftAmount, shiftApplication, actualCards, couponAmount,
                                 creditCardAmount, chequeAmount, actualGameCardAmount, actualcreditCardAmount, actualChequeAmount, actualCouponAmount, gamecardAmount);
            ShiftBL shiftBL = new ShiftBL(executionContext, shiftId);
            ShiftDTO shiftDTO = shiftBL.ShiftDTO;
            shiftBL.CloseShift(shiftAmount, cardCount, actualTickets, remarks, actualAmount, actualCards, gamecardAmount, creditCardAmount, chequeAmount, couponAmount,
                                    actualGameCardAmount, actualcreditCardAmount, actualChequeAmount, actualCouponAmount, shiftApplication, shiftTicketnumber, sqlTransaction);
            log.LogMethodExit();
        }

        private int GetSecurityPolicyId(int roleId)
        {
            log.LogMethodEntry(roleId);
            int result = UserRoleContainerList.GetSecurityPolicyId(executionContext, roleId);
            if (result == -1)
            {
                result = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DEFAULT_USER_SECURITY_POLICY", -1);
            }
            log.LogMethodExit(result);
            return result;
        }

        public void ChangeRoleId(int roleId)
        {
            log.LogMethodEntry(roleId);
            if (usersDTO.RoleId == roleId)
            {
                log.LogMethodExit(null, "No changes to user role");
                return;
            }
            ValidateRoleId(roleId);
            usersDTO.RoleId = roleId;
            log.LogMethodExit();
        }

        private void ValidateRoleId(int roleId)
        {
            log.LogMethodEntry(roleId);
            if (roleId == -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2474);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("User role is empty.", "User", "RoleId", errorMessage);
            }
            log.LogMethodExit();
        }

        public void ChangeUserName(string userName)
        {
            log.LogMethodEntry(userName);
            if (usersDTO.UserName == userName)
            {
                log.LogMethodExit(null, "No changes to user name");
                return;
            }
            ValidateUserName(userName);
            usersDTO.UserName = userName;
            log.LogMethodExit();
        }

        private void ValidateUserName(string userName)
        {
            log.LogMethodEntry(userName);
            if (string.IsNullOrWhiteSpace(userName))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2473);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("User name is empty.", "User", "UserName", errorMessage);
            }
            if (userName.Length > 100)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "User Name"), 100);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("User name greater than 100 characters.", "User", "UserName", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateLoginId(string loginId)
        {
            log.LogMethodEntry(loginId);
            if (string.IsNullOrWhiteSpace(loginId))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2476);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Login Id is empty.", "User", "LoginId", errorMessage);
            }
            if (loginId.Length > 100)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Login Id"), 100);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Login Id greater than 100 characters.", "User", "LoginId", errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Authenticates System User
        /// </summary>
        /// <param name="loginId">login Id</param>
        /// <param name="password">password</param>
        public void AuthenticateSystemUser(string loginId, string password)
        {
            log.LogMethodEntry();
            Core.GenericUtilities.EventLog.LogEvent(executionContext, Core.GenericUtilities.EventLog.LogType.Security, "System User Logging in loginId: " + loginId, "Authentication");
            if (UserStatus == UserStatus.DISABLED)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, (int)UserAuthenticationErrorType.USER_DISABLED);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.USER_DISABLED);
            }
            if (UserStatus == UserStatus.INACTIVE)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, (int)UserAuthenticationErrorType.USER_INACTIVE);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.USER_INACTIVE);
            }
            if (usersDTO.RoleId == -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, (int)UserAuthenticationErrorType.ROLE_NOT_DEFINED);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.ROLE_NOT_DEFINED);
            }
            int securityPolicyId = GetSecurityPolicyId(usersDTO.RoleId);
            DateTime now = ServerDateTime.Now;
            if (UserStatus == UserStatus.LOCKED)
            {
                int lockoutDuration = SecurityPolicyMasterList.GetLockoutDuration(executionContext, securityPolicyId);
                if (usersDTO.LockedOutTime == DateTime.MinValue || lockoutDuration > (now - usersDTO.LockedOutTime).TotalMinutes)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, (int)UserAuthenticationErrorType.USER_LOCKED_OUT);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.USER_LOCKED_OUT);
                }
            }
            UserEncryptionKey userEncryptionKey = new UserEncryptionKey(executionContext, loginId.ToLower());
            UserPasswordHash passwordHash = new UserPasswordHash(password, usersDTO.PasswordSalt, userEncryptionKey);
            UserPasswordHash userPasswordHash = new UserPasswordHash(usersDTO.PasswordHash);
            if (passwordHash != userPasswordHash)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, (int)UserAuthenticationErrorType.INVALID_PASSWORD);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.INVALID_PASSWORD);
            }
            bool passwordChangeRequired = usersDTO.PasswordChangeOnNextLogin;
            int passwordChangeFrequency = SecurityPolicyMasterList.GetPasswordChangeFrequency(executionContext, securityPolicyId);
            if (passwordChangeRequired == false && passwordChangeFrequency > 0)
            {
                passwordChangeRequired = passwordChangeFrequency < (now - Convert.ToDateTime(usersDTO.PasswordChangeDate)).TotalDays;
            }
            if (passwordChangeRequired)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, (int)UserAuthenticationErrorType.CHANGE_PASSWORD);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.CHANGE_PASSWORD);
            }
            Core.GenericUtilities.EventLog.LogEvent(executionContext, Core.GenericUtilities.EventLog.LogType.Security, Core.GenericUtilities.EventLog.EventType.Success, "Logged in System User " + usersDTO.LoginId, "Authentication");
            log.LogMethodExit();
        }

        /// <summary>
        /// Authenticate User
        /// </summary>
        /// <param name="loginId">login Id</param>
        /// <param name="cardNumber">card number</param>
        /// <param name="password">password</param>
        public void Authenticate(string loginId, string cardNumber, string password, string newPassword)
        {
            log.LogMethodEntry();
            Core.GenericUtilities.EventLog.LogEvent(executionContext, Core.GenericUtilities.EventLog.LogType.Security, "Logging in loginId: " + loginId + " cardNumber: " + cardNumber, "Authentication");
            if (UserStatus == UserStatus.DISABLED)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, (int)UserAuthenticationErrorType.USER_DISABLED);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.USER_DISABLED);
            }
            if (UserStatus == UserStatus.INACTIVE)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, (int)UserAuthenticationErrorType.USER_INACTIVE);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.USER_INACTIVE);
            }
            if (usersDTO.RoleId == -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, (int)UserAuthenticationErrorType.ROLE_NOT_DEFINED);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.ROLE_NOT_DEFINED);
            }
            int securityPolicyId = GetSecurityPolicyId(usersDTO.RoleId);
            DateTime now = ServerDateTime.Now;
            if (UserStatus == UserStatus.LOCKED)
            {
                int lockoutDuration = SecurityPolicyMasterList.GetLockoutDuration(executionContext, securityPolicyId);
                if (usersDTO.LockedOutTime == DateTime.MinValue || lockoutDuration > (now - usersDTO.LockedOutTime).TotalMinutes)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, (int)UserAuthenticationErrorType.USER_LOCKED_OUT);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.USER_LOCKED_OUT);
                }
            }
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                UserEncryptionKey userEncryptionKey = new UserEncryptionKey(executionContext, loginId.ToLower());
                UserPasswordHash passwordHash = new UserPasswordHash(password, usersDTO.PasswordSalt, userEncryptionKey);
                UserPasswordHash userPasswordHash = new UserPasswordHash(usersDTO.PasswordHash);
                if (passwordHash != userPasswordHash)
                {
                    if (UserStatus == UserStatus.LOCKED)
                    {
                        usersDTO.InvalidAccessAttempts = 0;
                        usersDTO.UserStatus = UserStatus.ACTIVE.ToString();
                    }
                    usersDTO.InvalidAccessAttempts++;
                    int invalidAttemptsBeforeLockout = SecurityPolicyMasterList.GetInvalidAttemptsBeforeLockout(executionContext, securityPolicyId);
                    if (invalidAttemptsBeforeLockout > 0 && usersDTO.InvalidAccessAttempts >= invalidAttemptsBeforeLockout)
                    {
                        usersDTO.UserStatus = UserStatus.LOCKED.ToString();
                        usersDTO.LockedOutTime = now;
                    }
                    using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                    {
                        transaction.BeginTransaction();
                        SaveImpl(transaction.SQLTrx, false);
                        transaction.EndTransaction();
                    }
                    string errorMessage = MessageContainerList.GetMessage(executionContext, (int)UserAuthenticationErrorType.INVALID_PASSWORD);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.INVALID_PASSWORD);
                }
            }
            else
            {
                if (IsValidUserIdentificationTag(cardNumber) == false)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, (int)UserAuthenticationErrorType.INVALID_USER_TAG);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.INVALID_USER_TAG);
                }
                bool authenticationRequiresTagAndLoginId = SecurityPolicyMasterList.GetAuthenticationRequiresTagAndLoginId(executionContext, securityPolicyId);
                if (authenticationRequiresTagAndLoginId && string.IsNullOrWhiteSpace(loginId))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, (int)UserAuthenticationErrorType.INVALID_LOGIN);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.INVALID_LOGIN);
                }
                if (string.IsNullOrWhiteSpace(loginId) == false &&
                    loginId != usersDTO.LoginId)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, (int)UserAuthenticationErrorType.INVALID_LOGIN);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.INVALID_LOGIN);
                }
            }
            bool passwordChangeRequired = usersDTO.PasswordChangeOnNextLogin;
            int passwordChangeFrequency = SecurityPolicyMasterList.GetPasswordChangeFrequency(executionContext, securityPolicyId);
            if (passwordChangeRequired == false && passwordChangeFrequency > 0)
            {
                passwordChangeRequired = passwordChangeFrequency < (now - Convert.ToDateTime(usersDTO.PasswordChangeDate)).TotalDays;
            }
            if (passwordChangeRequired)
            {
                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, (int)UserAuthenticationErrorType.CHANGE_PASSWORD);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new UserAuthenticationException(errorMessage, UserAuthenticationErrorType.CHANGE_PASSWORD);
                }
                else
                {
                    ChangePassword(newPassword);
                }
            }
            usersDTO.UserStatus = UserStatus.ACTIVE.ToString();
            usersDTO.InvalidAccessAttempts = 0;
            usersDTO.LastLoginTime = now;
            usersDTO.LogoutTime = DateTime.MinValue;
            using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
            {
                transaction.BeginTransaction();
                SaveImpl(transaction.SQLTrx, false);
                transaction.EndTransaction();
            }

            Core.GenericUtilities.EventLog.LogEvent(executionContext, Core.GenericUtilities.EventLog.LogType.Security, Core.GenericUtilities.EventLog.EventType.Success, "Logged in " + cardNumber + "/" + usersDTO.LoginId, "Authentication");
            log.LogMethodExit();
        }

        private bool IsValidUserIdentificationTag(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            bool result = false;
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                log.LogMethodExit(result, "Card number empty");
                return result;
            }
            if (usersDTO.UserIdentificationTagsDTOList == null ||
               usersDTO.UserIdentificationTagsDTOList.Any() == false)
            {
                log.LogMethodExit(result, "UserIdentificationTagsDTOList is empty");
                return result;
            }
            DateTime now = ServerDateTime.Now;
            result = usersDTO.UserIdentificationTagsDTOList.Any(x => x.CardNumber.ToLower() == cardNumber.ToLower() &&
                                                               (x.StartDate == DateTime.MinValue || x.StartDate <= now) &&
                                                               (x.EndDate == DateTime.MinValue || x.EndDate >= now));
            log.LogMethodExit(result);
            return result;
        }

        public UserStatus UserStatus
        {
            get
            {
                return (UserStatus)Enum.Parse(typeof(UserStatus), usersDTO.UserStatus);
            }
        }

        /// <summary>
        /// Gets the User Pass Phrase
        /// </summary>
        public static string GetUsersPassPhrase()
        {
            log.LogMethodEntry();
            string passPhrase;
            if (SiteContainerList.IsCorporate())
            {
                passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(SiteContainerList.GetMasterSiteId(), "USERS_ENCRYPTION_PASS_PHRASE");
            }
            else
            {
                passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(-1, "USERS_ENCRYPTION_PASS_PHRASE");
            }
            log.LogMethodExit("passPhrase");
            return passPhrase;
        }

        private DataAccessRuleDTO BuildDataAccessRuleDTO(int roleId, ExecutionContext executionContext)
        {
            log.LogMethodEntry(roleId, executionContext);
            DataAccessRuleDTO dataAccessRuleDTO = new DataAccessRuleDTO();

            if (roleId > -1)
            {
                List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> searchParameters = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
                searchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.ISACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.ROLE_ID, roleId.ToString()));

                UserRolesList usersList = new UserRolesList(executionContext);
                List<UserRolesDTO> userRolesDTOList = usersList.GetAllUserRoles(searchParameters, false, true);
                if (userRolesDTOList != null && userRolesDTOList.Count > 0)
                {
                    UserRolesDTO userRolesDTO = userRolesDTOList[0];
                    if (userRolesDTO.DataAccessRuleId > -1)
                    {
                        DataAccessRuleList dataAccessRuleList = new DataAccessRuleList(executionContext);
                        List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>> ruleSearchParameters = new List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>>();
                        ruleSearchParameters.Add(new KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>(DataAccessRuleDTO.SearchByDataAccessRuleParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        ruleSearchParameters.Add(new KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>(DataAccessRuleDTO.SearchByDataAccessRuleParameters.DATA_ACCESS_RULE_ID, userRolesDTO.DataAccessRuleId.ToString()));
                        ruleSearchParameters.Add(new KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>(DataAccessRuleDTO.SearchByDataAccessRuleParameters.ACTIVE_FLAG, "1"));
                        List<DataAccessRuleDTO> dataAccessRuleDTOList = dataAccessRuleList.GetAllDataAccessRule(ruleSearchParameters, true, true);
                        if (dataAccessRuleDTOList != null && dataAccessRuleDTOList.Count > 0)
                        {
                            dataAccessRuleDTO = dataAccessRuleDTOList[0];
                        }
                    }
                }
            }
            log.LogMethodExit(dataAccessRuleDTO);
            return dataAccessRuleDTO;
        }
        /// <summary>
        /// Method to determine if tracking is allowed or No
        /// </summary>
        /// <param name="userDTO"></param>
        /// <param name="clockInTime"></param>
        /// <param name="shiftConfigurationsDTO"></param>
        /// <returns></returns>
        public ShiftConfigurationsDTO GetShiftTrackingConfigurationForRole(int attendanecRoleId)
        {
            log.LogMethodEntry();
            bool result = false;
            DateTime businessDateTime = GetWorkShiftStartTime();
            int shiftConfigId = -1;
            if (usersDTO.ShiftConfigurationId > -1)
            {
                shiftConfigId = usersDTO.ShiftConfigurationId;
            }
            else
            {
                UserRoles userRoles = new UserRoles(executionContext, attendanecRoleId);
                if (userRoles.getUserRolesDTO.ShiftConfigurationId > -1)
                {
                    shiftConfigId = userRoles.getUserRolesDTO.ShiftConfigurationId;
                }
            }
            if (shiftConfigId > -1)
            {
                ShiftConfigurationsBL shiftConfigurationsBL = new ShiftConfigurationsBL(executionContext, shiftConfigId);
                ShiftConfigurationsDTO shiftConfigurationsDTO = shiftConfigurationsBL.ShiftConfigurationsDTO;
                AttendanceLogList attendanceLogBL = new AttendanceLogList(executionContext);
                List<KeyValuePair<AttendanceLogDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AttendanceLogDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AttendanceLogDTO.SearchByParameters, string>(AttendanceLogDTO.SearchByParameters.USER_ID, usersDTO.UserId.ToString()));
                searchParameters.Add(new KeyValuePair<AttendanceLogDTO.SearchByParameters, string>(AttendanceLogDTO.SearchByParameters.FROM_DATE, businessDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                List<AttendanceLogDTO> attendanceLogDTOList = attendanceLogBL.GetAllAttendanceUserList(searchParameters);
                if (attendanceLogDTOList != null && attendanceLogDTOList.Any())
                {
                    attendanceLogDTOList = attendanceLogDTOList.OrderBy(x => x.AttendanceLogId).ToList();
                    AttendanceLogDTO lastClockedInDTO = attendanceLogDTOList[attendanceLogDTOList.Count - 1];
                    if (//(lastClockedInDTO.Status != "On Break" && lastClockedInDTO.Status != "Clocked Out" && lastClockedInDTO.Status != "Manager approved Overtime") &&
                        (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_SHIFT_TRACKING").Equals("ALL")
                        || (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_SHIFT_TRACKING").Equals("ENABLEDSHIFTS")
                            && shiftConfigurationsBL.ShiftConfigurationsDTO.ShiftTrackAllowed)))
                    {
                        log.LogMethodExit(shiftConfigurationsDTO);
                        return shiftConfigurationsDTO;
                    }
                }
            }
            log.LogMethodExit(result);
            return null;
        }

        /// <summary>
        /// Method to get user clocked in time
        /// </summary>
        /// <returns>Clock in Time</returns>
        public DateTime GetClockInTime()
        {
            log.LogMethodEntry();
            DateTime workshiftStartTime = GetWorkShiftStartTime();
            AttendanceLogList attendanceLogBL = new AttendanceLogList(executionContext);
            List<KeyValuePair<AttendanceLogDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AttendanceLogDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AttendanceLogDTO.SearchByParameters, string>(AttendanceLogDTO.SearchByParameters.USER_ID, usersDTO.UserId.ToString()));
            searchParameters.Add(new KeyValuePair<AttendanceLogDTO.SearchByParameters, string>(AttendanceLogDTO.SearchByParameters.FROM_DATE, workshiftStartTime.Date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            List<AttendanceLogDTO> attendanceLogDTOList = attendanceLogBL.GetAllAttendanceUserList(searchParameters);
            if (attendanceLogDTOList != null || attendanceLogDTOList.Any())
            {
                attendanceLogDTOList = attendanceLogDTOList.OrderBy(x => x.Timestamp).ToList();
                log.LogMethodExit(attendanceLogDTOList[0].Timestamp);
                return attendanceLogDTOList[0].Timestamp;
            }
            log.LogMethodExit();
            return DateTime.MinValue;
        }

        /// <summary>
        /// GetAttendanceForDay
        /// </summary>
        /// <returns></returns>
        public AttendanceDTO GetAttendanceForDay()
        {
            log.LogMethodEntry();
            AttendanceDTO attendanceDTO = null;
            AttendanceList attendanceListBL = new AttendanceList(executionContext);
            DateTime workshiftStartTime = GetWorkShiftStartTime();
            List<KeyValuePair<AttendanceDTO.SearchByParameters, string>> searchByParams = new List<KeyValuePair<AttendanceDTO.SearchByParameters, string>>();
            searchByParams.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchByParams.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.USER_ID, usersDTO.UserId.ToString()));
            searchByParams.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.START_DATE, workshiftStartTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
            List<AttendanceDTO> attendanceDTOList = attendanceListBL.GetAllAttendance(searchByParams, true, true);
            if (attendanceDTOList != null && attendanceDTOList.Any())
            {
                attendanceDTO = attendanceDTOList[0];
            }
            log.LogMethodExit(attendanceDTO);
            return attendanceDTO;
        }

        private DateTime GetWorkShiftStartTime()
        {
            log.LogMethodEntry();
            DateTime currentTime = ServerDateTime.Now;
            DateTime workshiftStartTime = currentTime;

            int workshiftTime = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DEFAULT_WORKSHIFT_STARTTIME"));
            //workshiftStartTime = new DateTime(currentTime.ToUniversalTime().Year, currentTime.ToUniversalTime().Month, currentTime.ToUniversalTime().Day, 0, 0, 0, DateTimeKind.Local);
            //workshiftStartTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 0, 0, 0, DateTimeKind.Utc);

            WorkShiftDTO workShiftDTO = GetWorkShiftDTO();
            if (workShiftDTO != null)
            {
                WorkShiftUserDTO workShiftUserDTO = workShiftDTO.WorkShiftUsersDTOList.Find(x => x.UserId == usersDTO.UserId);
                if (workShiftUserDTO != null)
                {
                    workshiftStartTime = workShiftDTO.WorkShiftScheduleDTOList.Find(x => x.WorkShiftId == workShiftUserDTO.WorkShiftId).StartTime;
                }
            }
            if (currentTime.Hour < workshiftTime)
                workshiftStartTime = currentTime.AddDays(-1);
            workshiftStartTime = workshiftStartTime.Date.AddHours(workshiftTime);
            log.LogMethodExit(workshiftStartTime);
            return workshiftStartTime;
        }

        private WorkShiftDTO GetWorkShiftDTO()
        {
            log.LogMethodEntry();
            WorkShiftDTO workShiftDTO = null;
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            DateTime workshiftStartTime = lookupValuesList.GetServerDateTime();
            WorkShiftListBL workShiftBL = new WorkShiftListBL(executionContext);
            List<KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>> searchbyShiftParams = new List<KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>>();
            searchbyShiftParams.Add(new KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>(WorkShiftDTO.SearchByWorkShiftParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchbyShiftParams.Add(new KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>(WorkShiftDTO.SearchByWorkShiftParameters.STARTDATE, lookupValuesList.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            searchbyShiftParams.Add(new KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>(WorkShiftDTO.SearchByWorkShiftParameters.ENDDATE, lookupValuesList.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            List<WorkShiftDTO> workshiftDTOList = workShiftBL.GetWorkShiftDTOList(searchbyShiftParams, true, true);
            if (workshiftDTOList != null && workshiftDTOList.Any())
            {
                workShiftDTO = workshiftDTOList[0];
            }
            log.LogMethodExit(workShiftDTO);
            return workShiftDTO;
        }

        public bool IsManagerApprovedForClockIn(int selectedRoleId)
        {
            log.LogMethodEntry(selectedRoleId);
            bool managerApprovalRequired = false;
            UserToAttendanceRolesMapListBL userToAttendanceRolesMapListBL = new UserToAttendanceRolesMapListBL(executionContext);
            List<KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.ATTENDANCE_ROLE_ID, selectedRoleId.ToString()));
            searchParams.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.USER_ID, usersDTO.UserId.ToString()));
            searchParams.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<UserToAttendanceRolesMapDTO> userToAttendanceRolesMapDTOList = userToAttendanceRolesMapListBL.GetUserToAttendanceRolesMapDTOList(searchParams);
            if (userToAttendanceRolesMapDTOList != null && userToAttendanceRolesMapDTOList.Count > 0)
            {
                managerApprovalRequired = userToAttendanceRolesMapDTOList[0].ApprovalRequired;
            }
            else
            {
                AttendanceRolesList attendanceRolesList = new AttendanceRolesList(executionContext);
                List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>>();
                SearchParameters.Add(new KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>(AttendanceRoleDTO.SearchByParameters.ROLE_ID, usersDTO.RoleId.ToString()));
                SearchParameters.Add(new KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>(AttendanceRoleDTO.SearchByParameters.ATTENDANCE_ROLE_ID, selectedRoleId.ToString()));
                SearchParameters.Add(new KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>(AttendanceRoleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<AttendanceRoleDTO> attendanceRoleDtoList = attendanceRolesList.GetAttendanceRoles(SearchParameters);
                if (attendanceRoleDtoList != null && attendanceRoleDtoList.Count > 0)
                {
                    managerApprovalRequired = attendanceRoleDtoList[0].ApprovalRequired;
                }
            }
            log.LogMethodExit(managerApprovalRequired);
            return managerApprovalRequired;
        }

        public List<UserRolesDTO> GetAttendanceUserRoles()
        {
            log.LogMethodEntry();
            AttendanceRolesList attendanceRolesList = new AttendanceRolesList(executionContext);
            List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>(AttendanceRoleDTO.SearchByParameters.ROLE_ID, usersDTO.RoleId.ToString()));
            SearchParameters.Add(new KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>(AttendanceRoleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<AttendanceRoleDTO> attendanceRoleDtoList = attendanceRolesList.GetAttendanceRoles(SearchParameters);

            List<int> attendanceRoleIdList = new List<int>();
            if (attendanceRoleDtoList != null && attendanceRoleDtoList.Any())
            {
                attendanceRoleIdList.AddRange(attendanceRoleDtoList.Select(x => x.AttendanceRoleId).ToList());
            }
            UserToAttendanceRolesMapListBL userToAttendanceRolesMapListBL = new UserToAttendanceRolesMapListBL(executionContext);
            List<KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.USER_ID, usersDTO.UserId.ToString()));
            searchParams.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<UserToAttendanceRolesMapDTO> userToAttendanceRolesMapDTOList = userToAttendanceRolesMapListBL.GetUserToAttendanceRolesMapDTOList(searchParams);

            if (userToAttendanceRolesMapDTOList != null && userToAttendanceRolesMapDTOList.Any())
            {
                attendanceRoleIdList.AddRange(userToAttendanceRolesMapDTOList.Select(x => x.AttendanceRoleId).ToList());

            }

            // If no attendance roles are set up for the user, add the default role
            if (!attendanceRoleIdList.Any())
            {
                attendanceRoleIdList.Add(usersDTO.RoleId);
            }

            attendanceRoleIdList = attendanceRoleIdList.Distinct().ToList();
            string roleIdList = string.Join(",", attendanceRoleIdList);
            UserRolesList userRoleList = new UserRolesList();
            List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> SearchParameter = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
            SearchParameter.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.ROLE_ID_LIST, roleIdList));
            SearchParameter.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<UserRolesDTO> userRoleDtoList = userRoleList.GetAllUserRoles(SearchParameter);
            if (userRoleDtoList != null)
            {
                userRoleDtoList = userRoleDtoList.OrderBy(x => x.Role).ToList();
            }
            log.LogMethodExit(userRoleDtoList);
            return userRoleDtoList;
        }

        public string GetAttendancePrintReciept(bool isPrintAllowed, string attendanceType)
        {
            log.LogMethodEntry(isPrintAllowed, attendanceType);
            string printReciept = string.Empty;
            AttendanceDTO attendanceDTO = GetAttendanceForDay();
            AttendanceBL attendanceBL = new AttendanceBL(executionContext, attendanceDTO);
            if (isPrintAllowed)
            {
                printReciept = attendanceBL.GetClockInClockOutReciept(attendanceType, usersDTO.LoginId);
            }
            log.LogMethodExit(printReciept);
            return printReciept;
        }

        public void RecordAttendance(int selectedRoleId, int approverId, string status, int posMachineId, DateTime dateTime, int readerId, string mode, double tipvalue,
                                      int originalAttendanceLogId, string requestStatus, string approvedBy, DateTime? approvedDate, string type, string cardNumber,
                                      SqlTransaction sqlTransaction, bool printAllowed = false, string notes = null, string remarks = null)
        {
            log.LogMethodEntry(selectedRoleId, approverId, status, posMachineId, dateTime, sqlTransaction);
            try
            {
                int workShiftScheduleId = -1;
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                DateTime timeStamp = lookupValuesList.GetServerDateTime();

                //Get DayAttendance and workshift
                AttendanceDTO attendanceDTO = GetAttendanceForDay();
                DateTime workShiftStartTime = GetWorkShiftStartTime();
                WorkShiftDTO workShiftDTO = GetWorkShiftDTO();
                if (workShiftDTO != null)
                {
                    workShiftScheduleId = workShiftDTO.WorkShiftScheduleDTOList.Find(x => x.WorkShiftId == workShiftDTO.WorkShiftId).WorkShiftScheduleId;
                    workShiftStartTime = workShiftDTO.WorkShiftScheduleDTOList.Find(x => x.WorkShiftId == workShiftDTO.WorkShiftId).StartTime;
                }
                if (attendanceDTO == null)
                {
                    attendanceDTO = new AttendanceDTO(-1, usersDTO.UserId, workShiftStartTime, workShiftScheduleId, workShiftStartTime, 0, "OPEN", "Y");

                }
                if (selectedRoleId == -1)
                {
                    AttendanceLogDTO lastLoggedInLogDTO = attendanceDTO.AttendanceLogDTOList.Exists(x => x.AttendanceLogId >= 0) ?
                                                    attendanceDTO.AttendanceLogDTOList.FindAll(x => x.AttendanceLogId >= 0 && x.RequestStatus == string.Empty || x.RequestStatus == null ||
                                                    x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString()).OrderByDescending(y => y.Timestamp).ThenByDescending(z => z.AttendanceLogId).FirstOrDefault() : null;

                    if (lastLoggedInLogDTO != null && lastLoggedInLogDTO.Status != AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT))
                    {
                        selectedRoleId = lastLoggedInLogDTO.AttendanceRoleId;
                    }
                    else
                    {
                        selectedRoleId = usersDTO.RoleId;
                    }
                }
                if (approverId == -1)
                {
                    approverId = usersDTO.UserId;
                }
                AttendanceLogDTO attendanceLogDTO = new AttendanceLogDTO(-1, cardNumber, readerId, timeStamp, type, attendanceDTO.AttendanceId, mode, selectedRoleId,
                                                                          approverId, status, -1, posMachineId, tipvalue, "Y", originalAttendanceLogId, requestStatus,
                                                                          approvedBy, approvedDate, usersDTO.UserId, remarks, notes);
                attendanceDTO.AttendanceLogDTOList.Add(attendanceLogDTO);
                AttendanceBL attendanceBL = new AttendanceBL(executionContext, attendanceDTO);
                attendanceBL.RecordAttendance(sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

    }
    /// <summary>
    /// Manages the list of Users
    /// </summary>
    public class UsersList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<UsersDTO> usersList;

        /// <summary>
        /// default constructor
        /// </summary>
        public UsersList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public UsersList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.usersList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="usersList"></param>
        public UsersList(ExecutionContext executionContext, List<UsersDTO> usersList)
        {
            log.LogMethodEntry(executionContext, usersList);
            this.usersList = usersList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the users list
        /// </summary>
        public List<UsersDTO> GetAllUsers(List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters,
                                         bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            UsersDataHandler usersDataHandler = new UsersDataHandler(Users.GetUsersPassPhrase(), sqlTransaction);
            List<UsersDTO> usersDTOList = usersDataHandler.GetUsersList(searchParameters);
            if (usersDTOList != null && usersDTOList.Any())
            {
                foreach (var usersDTO in usersDTOList)
                {
                    usersDTO.Password = string.Empty;
                    usersDTO.PasswordHash = null;
                    usersDTO.PasswordSalt = string.Empty;
                    usersDTO.AcceptChanges();
                }
            }
            if (loadChildRecords)
            {
                Build(usersDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(usersDTOList);
            return usersDTOList;
        }

        private void Build(List<UsersDTO> usersDTOList, bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            Dictionary<int, UsersDTO> userIdIdUserDTODictionary = new Dictionary<int, UsersDTO>();
            List<int> userIdList = new List<int>();
            if (usersDTOList != null && usersDTOList.Any())
            {
                for (int i = 0; i < usersDTOList.Count; i++)
                {
                    if (usersDTOList[i].UserId == -1 ||
                        userIdIdUserDTODictionary.ContainsKey(usersDTOList[i].UserId))
                    {
                        continue;
                    }

                    userIdList.Add(usersDTOList[i].UserId);
                    userIdIdUserDTODictionary.Add(usersDTOList[i].UserId, usersDTOList[i]);
                }
                UserIdentificationTagListBL userIdTagsListBL = new UserIdentificationTagListBL();
                List<UserIdentificationTagsDTO> userIdentificationTagsDTOList = userIdTagsListBL.GetUserIdentificationTagDTOListOfUsers(userIdList, activeChildRecords, sqlTransaction);
                if (userIdentificationTagsDTOList != null && userIdentificationTagsDTOList.Any())
                {
                    log.LogVariableState("userIdentificationTagsDTOList", userIdentificationTagsDTOList);
                    foreach (UserIdentificationTagsDTO userIdentificationTagsDTO in userIdentificationTagsDTOList)
                    {
                        if (userIdIdUserDTODictionary.ContainsKey(userIdentificationTagsDTO.UserId))
                        {
                            if (userIdIdUserDTODictionary[userIdentificationTagsDTO.UserId].UserIdentificationTagsDTOList == null)
                            {
                                userIdIdUserDTODictionary[userIdentificationTagsDTO.UserId].UserIdentificationTagsDTOList = new List<UserIdentificationTagsDTO>();
                            }
                            userIdIdUserDTODictionary[userIdentificationTagsDTO.UserId].UserIdentificationTagsDTOList.Add(userIdentificationTagsDTO);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method should be used to Save and Update the Users details for Web Management Studio.
        /// </summary>
        public List<UsersDTO> Save()
        {
            log.LogMethodEntry();
            List<UsersDTO> savedUserDTOList = new List<UsersDTO>();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    if (usersList != null && usersList.Any())
                    {
                        parafaitDBTrx.BeginTransaction();
                        foreach (UsersDTO userDto in usersList)
                        {
                            Users users = new Users(executionContext, userDto);
                            users.Save(parafaitDBTrx.SQLTrx);
                            savedUserDTOList.Add(users.UserDTO);
                        }
                        parafaitDBTrx.EndTransaction();
                    }
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    if (sqlEx.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw;
                }

            }
            log.LogMethodExit(savedUserDTOList);
            return savedUserDTOList;
        }

        public DateTime? GetUserModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            UsersDataHandler usersDataHandler = new UsersDataHandler(Users.GetUsersPassPhrase());
            DateTime? result = usersDataHandler.GetUserModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

        public List<UsersDTO> GetAllClockedInUsers()
        {
            log.LogMethodEntry();
            List<UsersDTO> usersDTOsList = null;
            AttendanceLogList attendanceLogList = new AttendanceLogList(executionContext);
            List<AttendanceLogDTO> attendanceLogDTOList = attendanceLogList.GetAllClockedInUsers();
            if (attendanceLogDTOList != null && attendanceLogDTOList.Any())
            {
                string userIdList = string.Join(",", attendanceLogDTOList.Select(x => x.UserId));
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchByParams = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchByParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_ID_LIST, userIdList));
                searchByParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                usersDTOsList = GetAllUsers(searchByParams);
            }
            log.LogMethodExit(usersDTOsList);
            return usersDTOsList;
        }

        public List<UsersDTO> GetCurrentClockedInUsers()
        {
            log.LogMethodEntry();
            List<UsersDTO> usersDTOsList = null;
            string currentUserIdList = string.Empty;
            List<int> userIdList = new List<int>();
            AttendanceLogList attendanceLogList = new AttendanceLogList(executionContext);
            List<AttendanceLogDTO> attendanceLogDTOList = attendanceLogList.GetAllClockedInUsers();
            if (attendanceLogDTOList != null && attendanceLogDTOList.Any())
            {
                List<int> attendanceIDList = attendanceLogDTOList.Select(x => x.AttendanceId).Distinct().ToList();
                if (attendanceIDList.Any())
                {
                    AttendanceList attendanceList = new AttendanceList(executionContext);
                    string idList = string.Join(",", attendanceIDList);
                    List<KeyValuePair<AttendanceDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<AttendanceDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.ATTENDANCE_ID_LIST, idList));
                    searchParams.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    List<AttendanceDTO> attendanceDTOList = attendanceList.GetAllAttendance(searchParams, true);
                    foreach (AttendanceDTO attendanceDTO in attendanceDTOList)
                    {
                        AttendanceLogDTO lastLoggedInLogDTO = attendanceDTO.AttendanceLogDTOList.Exists(x => x.AttendanceLogId >= 0) ?
                                                     attendanceDTO.AttendanceLogDTOList.FindAll(x => x.AttendanceLogId >= 0 && x.RequestStatus == string.Empty || x.RequestStatus == null ||
                                                     x.RequestStatus == AttendanceLogDTO.AttendanceRequestStatus.Approved.ToString()).OrderByDescending(y => y.Timestamp).ThenByDescending(z => z.AttendanceLogId).FirstOrDefault() : null;

                        if (lastLoggedInLogDTO != null)
                        {
                            if (lastLoggedInLogDTO.Status != AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT)) // not clocked out
                            {
                                userIdList.Add(lastLoggedInLogDTO.UserId);
                            }
                        }
                    }
                }
                if (userIdList.Any())
                {
                    currentUserIdList = string.Join(",", userIdList);
                    List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchByParams = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                    searchByParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_ID_LIST, currentUserIdList));
                    searchByParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    usersDTOsList = GetAllUsers(searchByParams);
                }
            }
            log.LogMethodExit(usersDTOsList);
            return usersDTOsList;
        }


    }


}
