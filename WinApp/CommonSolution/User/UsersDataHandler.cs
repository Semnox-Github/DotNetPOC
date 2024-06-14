/********************************************************************************************
 * Project Name - Users Data Handler
 * Description  - Data handler of the user class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        25-Jan-2016   Raghuveera          Created 
 *********************************************************************************************
 *1.00        28-Jun-2016   Raghuveera          Modified 
 *2.70        26-Mar-2019   Guru S A            Booking phase 2 enhancement changes  
 *2.70        08-May-2019   Mushahid Faizan     Modified GetUsersList() to filter using IsActive parameter.
 *2.70.2      15-Jul-2019   Girish Kundar       Modified : Added GetSQLParameter(),SQL Injection Fix,Missed Who columns * 
 *2.70.2      11-Dec-2019   Jinto Thomas        Removed siteid from update query
 *2.70.3      29-03-2020    Girish Kundar       Modified : Removed password, passwordHash , passwordSalt from GetUsersDTO(DataRow) method
 *2.80.0      05-Jun-2020   Girish Kundar       Modified : Password change logic implemented for WMS 
 *2.90.0      09-Jul-2020   Akshay Gulaganji    Modified : Added fields DateOfBirth, ShiftConfigurationId
 *2.110.0     27-Nov-2020   Lakshminarayana     Modified : Changed as part of POS UI redesign. Implemented the new design principles
 *2.140.0     23-June-2021  Prashanth V         Modified : added a USER_ID_TAG as a search parameter into DBSearchParameters
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Users Data Handler - Handles insert, update and select of User Data objects
    /// </summary>
    public class UsersDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private string passPhrase;
        private const string SELECT_QUERY = @"SELECT u.user_id,	u.username,	u.password,	u.loginid,	u.role_id,	u.card_number,	u.active_flag,	u.last_login_time,	u.logout_time,	u.finger_print,
                                                u.fp_template,	u.override_fingerprint,	u.POSTypeId,	u.Guid,	u.site_id,	u.SynchStatus,	u.LastUpdatedBy,	u.LastUpdatedDate,	u.DepartmentId,	
                                                u.EmpStartDate,	u.EmpEndDate,	u.EmpEndReason,	u.ManagerId,	u.EmpLastName,	u.EmpNumber,	u.CompanyAdministrator,	u.FingerPrint,	u.FingerNumber,	
                                                u.PasswordHash,	u.UserStatus,	u.CreationDate,	u.CreatedBy,	u.PasswordChangeDate,	u.InvalidAccessAttempts,	u.LockedOutTime,	u.PasswordChangeOnNextLogin,	
                                                u.PasswordSalt,	u.email,	u.MasterEntityId,	u.ShiftConfigurationId,	CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@passPhrase,u.DateOfBirth)) AS DateOfBirth,	
                                                CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@passPhrase,u.PhoneNumber)) AS PhoneNumber
                                              FROM users AS u ";
        private static readonly Dictionary<UsersDTO.SearchByUserParameters, string> DBSearchParameters = new Dictionary<UsersDTO.SearchByUserParameters, string>
            {
                {UsersDTO.SearchByUserParameters.USER_ID, "u.user_id"},
                {UsersDTO.SearchByUserParameters.USER_NAME, "u.username"},
                {UsersDTO.SearchByUserParameters.LOGIN_ID, "u.loginid"},
                {UsersDTO.SearchByUserParameters.EMAIL, "u.email"},
                {UsersDTO.SearchByUserParameters.USER_STATUS, "u.UserStatus"},
                {UsersDTO.SearchByUserParameters.CARD_NUMBER, "u.card_number"},
                {UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "u.active_flag"},
                {UsersDTO.SearchByUserParameters.LAST_UPDATE_DATE, "u.LastUpdatedDate"},
                {UsersDTO.SearchByUserParameters.SITE_ID, "u.site_id"},//Modification  on 28-Jun-2016 added site id
                {UsersDTO.SearchByUserParameters.MASTER_ENTITY_ID, "u.MasterEntityId"},
                {UsersDTO.SearchByUserParameters.EMP_NUMBER, "u.EmpNumber"}, //Added on 28-Oct-2016
                {UsersDTO.SearchByUserParameters.ROLE_ID, "u.role_id"}, //Added on 28-Oct-2016
                {UsersDTO.SearchByUserParameters.DEPARTMENT_ID, "u.departmentId"},
                {UsersDTO.SearchByUserParameters.ROLE_NOT_IN, "u.role_id"},
                 {UsersDTO.SearchByUserParameters.ROLE_ID_LIST, "u.role_id"},
                 {UsersDTO.SearchByUserParameters.USER_ID_LIST, "u.user_id"},
                {UsersDTO.SearchByUserParameters.SHIFT_CONFIGURATION_ID, "u.ShiftConfigurationId"},
                {UsersDTO.SearchByUserParameters.USER_ID_TAG,""} 
            };


        /// <summary>
        /// Default constructor of UserDataHandler class
        /// </summary>
        public UsersDataHandler(string passPhrase, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction, "passPhrase");
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            this.passPhrase = passPhrase;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Users Record.
        /// </summary>
        /// <param name="usersDTO">UsersDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(UsersDTO usersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(usersDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@userId", usersDTO.UserId, true);
            ParametersHelper.ParameterHelper(parameters, "@roleId", usersDTO.RoleId, true);
            ParametersHelper.ParameterHelper(parameters, "@pOSTypeId", usersDTO.PosTypeId, true);
            ParametersHelper.ParameterHelper(parameters, "@departmentId", usersDTO.DepartmentId, true);
            ParametersHelper.ParameterHelper(parameters, "@managerId", usersDTO.ManagerId, true);
            ParametersHelper.ParameterHelper(parameters, "@username", string.IsNullOrEmpty(usersDTO.UserName) ? DBNull.Value : (object)usersDTO.UserName);
            ParametersHelper.ParameterHelper(parameters, "@loginid", string.IsNullOrEmpty(usersDTO.LoginId) ? DBNull.Value : (object)usersDTO.LoginId);
            ParametersHelper.ParameterHelper(parameters, "@cardNumber", string.IsNullOrEmpty(usersDTO.CardNumber) ? DBNull.Value : (object)usersDTO.CardNumber);
            ParametersHelper.ParameterHelper(parameters, "@lastLoginTime", usersDTO.LastLoginTime == DateTime.MinValue ? DBNull.Value : (object)usersDTO.LastLoginTime);
            ParametersHelper.ParameterHelper(parameters, "@logoutTime", usersDTO.LogoutTime == DateTime.MinValue ? DBNull.Value : (object)usersDTO.LogoutTime);
            ParametersHelper.ParameterHelper(parameters, "@empStartDate", usersDTO.EmpStartDate == DateTime.MinValue ? DBNull.Value : (object)usersDTO.EmpStartDate);
            ParametersHelper.ParameterHelper(parameters, "@empEndDate", usersDTO.EmpEndDate == DateTime.MinValue ? DBNull.Value : (object)usersDTO.EmpEndDate);
            ParametersHelper.ParameterHelper(parameters, "@lockedOutTime", usersDTO.LockedOutTime == DateTime.MinValue ? DBNull.Value : (object)usersDTO.LockedOutTime);
            ParametersHelper.ParameterHelper(parameters, "@passwordChangeDate", usersDTO.PasswordChangeDate == DateTime.MinValue ? DBNull.Value : (object)usersDTO.PasswordChangeDate);
            ParametersHelper.ParameterHelper(parameters, "@overrideFingerPrint", string.IsNullOrEmpty(usersDTO.OverrideFingerPrint) ? DBNull.Value : (object)usersDTO.OverrideFingerPrint);
            ParametersHelper.ParameterHelper(parameters, "@empEndReason", string.IsNullOrEmpty(usersDTO.EmpEndReason) ? DBNull.Value : (object)usersDTO.EmpEndReason);
            ParametersHelper.ParameterHelper(parameters, "@empLastName", string.IsNullOrEmpty(usersDTO.EmpLastName) ? DBNull.Value : (object)usersDTO.EmpLastName);
            ParametersHelper.ParameterHelper(parameters, "@empNumber", string.IsNullOrEmpty(usersDTO.EmpNumber) ? DBNull.Value : (object)usersDTO.EmpNumber);
            ParametersHelper.ParameterHelper(parameters, "@companyAdministrator", string.IsNullOrEmpty(usersDTO.CompanyAdministrator) ? DBNull.Value : (object)usersDTO.CompanyAdministrator);
            ParametersHelper.ParameterHelper(parameters, "@userStatus", string.IsNullOrEmpty(usersDTO.UserStatus) ? DBNull.Value : (object)usersDTO.UserStatus);
            ParametersHelper.ParameterHelper(parameters, "@passwordSalt", string.IsNullOrEmpty(usersDTO.PasswordSalt) ? DBNull.Value : (object)usersDTO.PasswordSalt);
            ParametersHelper.ParameterHelper(parameters, "@password", string.IsNullOrEmpty(usersDTO.Password) ? DBNull.Value : (object)usersDTO.Password);
            ParametersHelper.ParameterHelper(parameters, "@email", string.IsNullOrEmpty(usersDTO.Email) ? DBNull.Value : (object)usersDTO.Email);
            ParametersHelper.ParameterHelper(parameters, "@activeFlag", (usersDTO.IsActive) ? "Y" : "N");
            ParametersHelper.ParameterHelper(parameters, "@lastUpdatedBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@createdBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@siteId", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@fingerNumber", usersDTO.FingerNumber == -1 ? DBNull.Value : (object)usersDTO.FingerNumber);
            ParametersHelper.ParameterHelper(parameters, "@invalidAccessAttempts", usersDTO.InvalidAccessAttempts == -1 ? DBNull.Value : (object)usersDTO.InvalidAccessAttempts);
            ParametersHelper.ParameterHelper(parameters, "@passwordChangeOnNextLogin", usersDTO.PasswordChangeOnNextLogin);
            ParametersHelper.ParameterHelper(parameters, "@masterEntityId", usersDTO.MasterEntityId, true);
            //ParametersHelper.ParameterHelper(parameters, "@passwordHash", usersDTO.PasswordHash == null ? DBNull.Value : (object)usersDTO.PasswordHash);
            SqlParameter parameter = new SqlParameter("@passwordHash", SqlDbType.VarBinary);
            if (usersDTO.PasswordHash == null || usersDTO.PasswordHash.Length <= 0)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = usersDTO.PasswordHash;
            }
            parameters.Add(dataAccessHandler.GetSQLParameter("@passPhrase", passPhrase));
            ParametersHelper.ParameterHelper(parameters, "@dateOfBirth", usersDTO.DateOfBirth == null ? DBNull.Value : (object)usersDTO.DateOfBirth.Value.ToString("yyyy/MM/dd HH:mm:ss"));

            ParametersHelper.ParameterHelper(parameters, "@phoneNumber", usersDTO.PhoneNumber, true);
            ParametersHelper.ParameterHelper(parameters, "@shiftConfigurationId", usersDTO.ShiftConfigurationId, true);
            parameters.Add(parameter);
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the user record to the database
        /// </summary>
        /// <param name="userDTO">UsersDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns UsersDTO</returns>
        public UsersDTO InsertUser(UsersDTO userDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userDTO, loginId, siteId);
            string insertQuery = @"insert into users 
                                                        (                                                         
                                                        username,
                                                        password,
                                                        loginid,
                                                        role_id,
                                                        card_number,                                                        
                                                        last_login_time,
                                                        logout_time,
                                                        override_fingerprint,
                                                        POSTypeId,
                                                        DepartmentId,
                                                        EmpStartDate,
                                                        EmpEndDate,
                                                        EmpEndReason,
                                                        ManagerId,
                                                        EmpLastName,
                                                        EmpNumber,
                                                        CompanyAdministrator,
                                                        FingerNumber,
                                                        UserStatus,
                                                        PasswordChangeDate,
                                                        InvalidAccessAttempts,
                                                        LockedOutTime,
                                                        PasswordChangeOnNextLogin,
                                                        PasswordHash,
                                                        PasswordSalt,
                                                        MasterEntityId,
                                                        email,
                                                        active_flag,
                                                        site_id,
                                                        Guid,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                          LastUpdatedDate,
                                                        DateOfBirth,
                                                        PhoneNumber,
                                                        ShiftConfigurationId
                                                      ) 
                                                values 
                                                        (                                                        
                                                        @username,
                                                        @password,
                                                        @loginid,
                                                        @roleId,
                                                        @cardNumber,                                                        
                                                        @lastLoginTime,
                                                        @logoutTime,
                                                        @overrideFingerPrint,
                                                        @pOSTypeId,
                                                        @departmentId,
                                                        @empStartDate,
                                                        @empEndDate,
                                                        @empEndReason,
                                                        @managerId,
                                                        @empLastName,
                                                        @empNumber,
                                                        @companyAdministrator,                                                       
                                                        @fingerNumber,
                                                        @userStatus,
                                                        @passwordChangeDate,
                                                        @invalidAccessAttempts,
                                                        @lockedOutTime,
                                                        @passwordChangeOnNextLogin,
                                                        @passwordHash,
                                                        @passwordSalt,
                                                        @masterEntityId,
                                                        @email,
                                                        @activeFlag,
                                                        @siteId,
                                                        NewId(),
                                                        @createdBy,
                                                        GetDate(),
                                                        @lastUpdatedBy,
                                                         GetDate(),
                                                        ENCRYPTBYPASSPHRASE(@passPhrase, @dateOfBirth),
                                                        ENCRYPTBYPASSPHRASE(@passPhrase, @phoneNumber),
                                                        @shiftConfigurationId
                                          )SELECT  * from users where user_id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(userDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserDTO(userDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting userDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userDTO);
            return userDTO;
        }

        internal bool IsUserWithSameManagerIdExists(int userId)
        {
            log.LogMethodEntry(userId);
            bool result = dataAccessHandler.executeScalar("select top 1 1 from users where ManagerId = user_id and user_id != @userId", new[]{ new SqlParameter("@userId", userId) }, sqlTransaction) != null;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Updates the user record
        /// </summary>
        /// <param name="userDTO">UsersDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the UsersDTO</returns>
        public UsersDTO UpdateUser(UsersDTO userDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userDTO,  loginId,  siteId);
            string updateQuery = @"update Users with(xlock , holdlock, rowlock )
                                         set username=@username,
                                             loginid=@loginId,
                                             role_id=@roleId,
                                             card_number=@cardNumber,                                                        
                                             last_login_time=@lastLoginTime,
                                             logout_time=@logoutTime,
                                             override_fingerprint=@overrideFingerPrint,
                                             POSTypeId=@pOSTypeId,
                                             DepartmentId=@departmentId,
                                             EmpStartDate=@empStartDate,
                                             EmpEndDate=@empEndDate,
                                             EmpEndReason=@empEndReason,
                                             ManagerId=@managerId,
                                             EmpLastName=@empLastName,
                                             EmpNumber=@empNumber,
                                             CompanyAdministrator=@companyAdministrator,
                                             FingerNumber=@fingerNumber,
                                             UserStatus=@userStatus,
                                             PasswordChangeDate=@passwordChangeDate,
                                             InvalidAccessAttempts=@invalidAccessAttempts,
                                             LockedOutTime=@lockedOutTime,
                                             PasswordChangeOnNextLogin=@passwordChangeOnNextLogin,
                                             --password=@password,
                                             PasswordHash=@passwordHash,
                                             PasswordSalt=@passwordSalt,
                                             MasterEntityId=@masterEntityId,
                                             email=@email,
                                             active_flag=@activeFlag,
                                             -- site_id=@siteId,
                                             LastUpdatedBy=@lastUpdatedBy,
                                             LastUpdatedDate=getdate(),
                                             DateOfBirth=ENCRYPTBYPASSPHRASE(@passPhrase, @dateOfBirth),
                                             PhoneNumber=ENCRYPTBYPASSPHRASE(@passPhrase, @phoneNumber),
                                             ShiftConfigurationId=@shiftConfigurationId
                                          where user_id = @userId  
                                    SELECT  * from users with(xlock , holdlock, rowlock ) where user_id = @userId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(userDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserDTO(userDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating userDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userDTO);
            return userDTO;
        }

        /// <summary>
        /// Updates the user record by system process
        /// </summary>
        /// <param name="userDTO">UsersDTO type parameter</param>
        /// <returns>Returns the UsersDTO</returns>
        public UsersDTO UpdateUser(UsersDTO userDTO)
        {
            log.LogMethodEntry(userDTO);
            string updateQuery = @"update Users with(xlock , holdlock, rowlock )
                                         set username=@username,
                                             loginid=@loginId,
                                             role_id=@roleId,
                                             card_number=@cardNumber,                                                        
                                             last_login_time=@lastLoginTime,
                                             logout_time=@logoutTime,
                                             override_fingerprint=@overrideFingerPrint,
                                             POSTypeId=@pOSTypeId,
                                             DepartmentId=@departmentId,
                                             EmpStartDate=@empStartDate,
                                             EmpEndDate=@empEndDate,
                                             EmpEndReason=@empEndReason,
                                             ManagerId=@managerId,
                                             EmpLastName=@empLastName,
                                             EmpNumber=@empNumber,
                                             CompanyAdministrator=@companyAdministrator,
                                             FingerNumber=@fingerNumber,
                                             UserStatus=@userStatus,
                                             PasswordChangeDate=@passwordChangeDate,
                                             InvalidAccessAttempts=@invalidAccessAttempts,
                                             LockedOutTime=@lockedOutTime,
                                             PasswordChangeOnNextLogin=@passwordChangeOnNextLogin,
                                             password=@password,
                                             PasswordHash=@passwordHash,
                                             PasswordSalt=@passwordSalt,
                                             MasterEntityId=@masterEntityId,
                                             email=@email,
                                             active_flag=@activeFlag,
                                             DateOfBirth=ENCRYPTBYPASSPHRASE(@passPhrase, @dateOfBirth),
                                             PhoneNumber=ENCRYPTBYPASSPHRASE(@passPhrase, @phoneNumber),
                                             ShiftConfigurationId=@shiftConfigurationId
                                          where user_id = @userId  
                                    SELECT  * from users with(xlock , holdlock, rowlock ) where user_id = @userId  ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(userDTO, string.Empty, -1).ToArray(), sqlTransaction);
                RefreshUserDTO(userDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating userDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userDTO);
            return userDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="usersDTO">AttendanceDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshUserDTO(UsersDTO usersDTO, DataTable dt)
        {
            log.LogMethodEntry(usersDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                usersDTO.UserId = Convert.ToInt32(dt.Rows[0]["user_id"]);
                usersDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                usersDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                usersDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                usersDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                usersDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                usersDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to UsersDTO class type
        /// </summary>
        /// <param name="userDataRow">UsersDTO DataRow</param>
        /// <returns>Returns UsersDTO</returns>
        private UsersDTO GetUsersDTO(DataRow userDataRow)
        {
            log.LogMethodEntry(userDataRow);
            UsersDTO userDataObject = new UsersDTO(Convert.ToInt32(userDataRow["user_id"]),
                                                    userDataRow["username"].ToString(),
                                                    userDataRow["loginid"].ToString(),
                                                    userDataRow["role_id"] == DBNull.Value ? -1 : Convert.ToInt32(userDataRow["role_id"]),
                                                    userDataRow["card_number"] == DBNull.Value ? string.Empty : userDataRow["card_number"].ToString(),
                                                    userDataRow["last_login_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userDataRow["last_login_time"]),
                                                    userDataRow["logout_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userDataRow["logout_time"]),
                                                    userDataRow["override_fingerprint"] == DBNull.Value ? string.Empty : userDataRow["override_fingerprint"].ToString(),
                                                    userDataRow["POSTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(userDataRow["POSTypeId"]),
                                                    userDataRow["DepartmentId"] == DBNull.Value ? -1 : Convert.ToInt32(userDataRow["DepartmentId"]), string.Empty,
                                                    userDataRow["EmpStartDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userDataRow["EmpStartDate"]),
                                                    userDataRow["EmpEndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userDataRow["EmpEndDate"]),
                                                    userDataRow["EmpEndReason"] == DBNull.Value ? string.Empty : userDataRow["EmpEndReason"].ToString(),
                                                    userDataRow["ManagerId"] == DBNull.Value ? -1 : Convert.ToInt32(userDataRow["ManagerId"]),
                                                    userDataRow["EmpLastName"] == DBNull.Value ? string.Empty : userDataRow["EmpLastName"].ToString(),
                                                    userDataRow["EmpNumber"] == DBNull.Value ? string.Empty : userDataRow["EmpNumber"].ToString(),
                                                    userDataRow["CompanyAdministrator"] == DBNull.Value ? string.Empty : userDataRow["CompanyAdministrator"].ToString(),
                                                    userDataRow["UserStatus"] == DBNull.Value ? string.Empty : userDataRow["UserStatus"].ToString(),
                                                    userDataRow["PasswordChangeDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userDataRow["PasswordChangeDate"]),
                                                    userDataRow["InvalidAccessAttempts"] == DBNull.Value ? -1 : Convert.ToInt32(userDataRow["InvalidAccessAttempts"]),
                                                    userDataRow["LockedOutTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userDataRow["LockedOutTime"]),
                                                    userDataRow["PasswordChangeOnNextLogin"] == DBNull.Value ? false : Convert.ToBoolean(userDataRow["PasswordChangeOnNextLogin"]),
                                                    userDataRow["PasswordHash"] == DBNull.Value ? null : userDataRow["PasswordHash"] as byte[],
                                                    userDataRow["PasswordSalt"] == DBNull.Value ? string.Empty : userDataRow["PasswordSalt"].ToString(),
                                                    userDataRow["password"] == DBNull.Value ? string.Empty : userDataRow["password"].ToString(),
                                                    userDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(userDataRow["MasterEntityId"]),
                                                    userDataRow["email"] == DBNull.Value ? string.Empty : userDataRow["email"].ToString(),
                                                    userDataRow["active_flag"].ToString(),
                                                    userDataRow["Guid"] == DBNull.Value ? string.Empty : userDataRow["Guid"].ToString(),
                                                    userDataRow["CreatedBy"] == DBNull.Value ? string.Empty : userDataRow["CreatedBy"].ToString(),
                                                    userDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userDataRow["CreationDate"]),
                                                    userDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : userDataRow["LastUpdatedBy"].ToString(),
                                                    userDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userDataRow["LastupdatedDate"]),
                                                    userDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(userDataRow["site_id"]),
                                                    userDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(userDataRow["SynchStatus"]),
                                                    userDataRow["DateOfBirth"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(userDataRow["DateOfBirth"]),
                                                    userDataRow["PhoneNumber"] == DBNull.Value ? string.Empty : userDataRow["PhoneNumber"].ToString(),
                                                    userDataRow["ShiftConfigurationId"] == DBNull.Value ? -1 : Convert.ToInt32(userDataRow["ShiftConfigurationId"])
                                                    );
            log.LogMethodExit(userDataObject);
            return userDataObject;
        }

        /// <summary>
        /// Gets the user data of passed userId
        /// </summary>
        /// <param name="userId">integer type parameter</param>
        /// <returns>Returns UsersDTO</returns>
        internal UsersDTO GetUsers(int userId)
        {
            log.LogMethodEntry(userId);
            UsersDTO UserDataObject = null ;
            string selectUserQuery =SELECT_QUERY + "   where u.user_id = @userId";
            SqlParameter[] selectUserParameters = new SqlParameter[2];
            selectUserParameters[0] = new SqlParameter("@userId", userId);
            selectUserParameters[1] = new SqlParameter("@passPhrase", passPhrase);
            DataTable users = dataAccessHandler.executeSelectQuery(selectUserQuery, selectUserParameters , sqlTransaction);
            if (users.Rows.Count > 0)
            {
                DataRow userRow = users.Rows[0];
                UserDataObject = GetUsersDTO(userRow);
            }
            log.LogMethodExit(UserDataObject);
            return UserDataObject;

        }

        /// <summary>
        /// Gets the user data of passed loginId and siteId
        /// </summary>
        /// <param name="userId">integer type parameter</param>
        /// <returns>Returns UsersDTO</returns>
        internal UsersDTO GetUsers(string loginId, int siteId)
        {
            log.LogMethodEntry(loginId, siteId);
            UsersDTO UserDataObject = null ;
            string selectUserQuery =SELECT_QUERY + " where u.loginid = @loginId and (u.site_id = @siteId or @siteId = -1)";
            SqlParameter[] selectUserParameters = new SqlParameter[3];
            selectUserParameters[0] = new SqlParameter("@loginId", loginId);
            selectUserParameters[1] = new SqlParameter("@siteId", siteId);
            selectUserParameters[2] = new SqlParameter("@passPhrase", passPhrase);
            DataTable users = dataAccessHandler.executeSelectQuery(selectUserQuery, selectUserParameters , sqlTransaction);
            if (users.Rows.Count > 0)
            {
                DataRow userRow = users.Rows[0];
                UserDataObject = GetUsersDTO(userRow);
            }
            log.LogMethodExit(UserDataObject);
            return UserDataObject;

        }

        /// <summary>
        /// Gets the UsersDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTrx">SqlTransaction object</param>
        /// <returns>Returns the list of UsersDTO matching the search criteria</returns>
        public List<UsersDTO> GetUsersList(List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectUsersQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<UsersDTO> usersList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<UsersDTO.SearchByUserParameters, string> searchParameter in searchParameters)
                {
                    string joinOperartor = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(UsersDTO.SearchByUserParameters.LAST_UPDATE_DATE))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " >" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(UsersDTO.SearchByUserParameters.USER_ID) || searchParameter.Key.Equals(UsersDTO.SearchByUserParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(UsersDTO.SearchByUserParameters.SITE_ID))//starts:Modification  on 28-Jun-2016 added site id for filter
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key) + " OR -1=" + dataAccessHandler.GetParameterName(searchParameter.Key) + ")");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }//Ends:Modification  on 28-Jun-2016 Added site id
                        else if (searchParameter.Key.Equals(UsersDTO.SearchByUserParameters.LOGIN_ID) ||
                                  searchParameter.Key.Equals(UsersDTO.SearchByUserParameters.USER_NAME) ||
                                  searchParameter.Key.Equals(UsersDTO.SearchByUserParameters.USER_STATUS) ||
                                  searchParameter.Key.Equals(UsersDTO.SearchByUserParameters.CARD_NUMBER) ||
                                  searchParameter.Key.Equals(UsersDTO.SearchByUserParameters.EMAIL) ||
                                   searchParameter.Key.Equals(UsersDTO.SearchByUserParameters.EMP_NUMBER) ||
                                    searchParameter.Key.Equals(UsersDTO.SearchByUserParameters.ROLE_ID) ||
                                    searchParameter.Key.Equals(UsersDTO.SearchByUserParameters.DEPARTMENT_ID) ||
                                    searchParameter.Key.Equals(UsersDTO.SearchByUserParameters.SHIFT_CONFIGURATION_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == UsersDTO.SearchByUserParameters.ROLE_ID_LIST|| searchParameter.Key == UsersDTO.SearchByUserParameters.USER_ID_LIST)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == UsersDTO.SearchByUserParameters.ACTIVE_FLAG)
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N"));
                        }
                        else if (searchParameter.Key.Equals(UsersDTO.SearchByUserParameters.ROLE_NOT_IN))
                        {
                            query.Append(joinOperartor + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",-1) NOT IN (select role_id from user_roles where role in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ")) ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(UsersDTO.SearchByUserParameters.USER_ID_TAG))
                        {
                            query.Append(joinOperartor + "exists(select 1 from useridentificationtags uid where uid.userid = u.user_id and uid.cardnumber =" + dataAccessHandler.GetParameterName(UsersDTO.SearchByUserParameters.CARD_NUMBER));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(UsersDTO.SearchByUserParameters.CARD_NUMBER), (searchParameters.Find(x => x.Key == UsersDTO.SearchByUserParameters.CARD_NUMBER)).Value));
                        }
                        count++;
                    }
                    else if (searchParameter.Key.Equals(UsersDTO.SearchByUserParameters.USER_FIRST_OR_LAST_NAME))
                    {
                        query.Append(" and (Isnull(u.username,'') like " + "N'%" + searchParameter.Value
                            + "%'" + " or Isnull(u.EmpLastName, '') like " + "N'%" + searchParameter.Value + "')");
                        parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }

                if (searchParameters.Count > 0)
                    selectUsersQuery = selectUsersQuery + query;
                selectUsersQuery = selectUsersQuery + " Order by username";
            }


            parameters.Add(dataAccessHandler.GetSQLParameter("@passPhrase", passPhrase));
            DataTable usersData = dataAccessHandler.executeSelectQuery(selectUsersQuery, parameters.ToArray(), sqlTransaction);
            if (usersData.Rows.Count > 0)
            {
                usersList = new List<UsersDTO>();
                foreach (DataRow usersDataRow in usersData.Rows)
                {
                    UsersDTO usersDataObject = GetUsersDTO(usersDataRow);
                    usersList.Add(usersDataObject);
                }
            }
                log.LogMethodExit(usersList);
                return usersList;           
        }

        internal DateTime? GetUserModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(LastupdatedDate) LastUpdatedDate from user_roles WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastupdatedDate) LastUpdatedDate from users WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastupdatedDate) LastUpdatedDate from UserIdentificationTags WHERE (site_id = @siteId or @siteId = -1)
                            ) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
