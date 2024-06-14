/********************************************************************************************
 * Project Name - UserToAttendanceRolesMap
 * Description  - Business logic file for  User To AttendanceRoles Map
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90.0      06-Jul-2020   Akshay Gulaganji        Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Business logic for UserToAttendanceRolesMap class
    /// </summary>
    public class UserToAttendanceRolesMapBL
    {
        private UserToAttendanceRolesMapDTO userToAttendanceRolesMapDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of UserToAttendanceRolesMapBL class
        /// </summary>
        private UserToAttendanceRolesMapBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates UserToAttendanceRolesMapBL object using the UserToAttendanceRolesMapDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="userToAttendanceRolesMapDTO">UserToAttendanceRolesMapDTO object</param>
        public UserToAttendanceRolesMapBL(ExecutionContext executionContext, UserToAttendanceRolesMapDTO userToAttendanceRolesMapDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, userToAttendanceRolesMapDTO);
            this.userToAttendanceRolesMapDTO = userToAttendanceRolesMapDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the userToAttendanceRolesMapId as the parameter
        /// Would fetch the UserToAttendanceRolesMap object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="userToAttendanceRolesMapId">id - userToAttendanceRolesMapId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public UserToAttendanceRolesMapBL(ExecutionContext executionContext, int userToAttendanceRolesMapId, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, userToAttendanceRolesMapId, sqlTransaction);
            UserToAttendanceRolesMapDataHandler userToAttendanceRolesMapDataHandler = new UserToAttendanceRolesMapDataHandler(sqlTransaction);
            userToAttendanceRolesMapDTO = userToAttendanceRolesMapDataHandler.GetUserToAttendanceRolesMapDTO(userToAttendanceRolesMapId);
            if (userToAttendanceRolesMapDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "UserToAttendanceRolesMapDTO", userToAttendanceRolesMapId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the UserToAttendanceRolesMapDTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (userToAttendanceRolesMapDTO.IsChanged == false && userToAttendanceRolesMapDTO.UserToAttendanceRolesMapId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            UserToAttendanceRolesMapDataHandler userToAttendanceRolesMapDataHandler = new UserToAttendanceRolesMapDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (userToAttendanceRolesMapDTO.UserToAttendanceRolesMapId < 0)
            {
                log.LogVariableState("UserToAttendanceRolesMapDTO", userToAttendanceRolesMapDTO);
                userToAttendanceRolesMapDTO = userToAttendanceRolesMapDataHandler.Insert(userToAttendanceRolesMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(userToAttendanceRolesMapDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("user_roles", userToAttendanceRolesMapDTO.Guid, sqlTransaction);
                }
                userToAttendanceRolesMapDTO.AcceptChanges();
            }
            else if (userToAttendanceRolesMapDTO.IsChanged)
            {
                log.LogVariableState("UserToAttendanceRolesMapDTO", userToAttendanceRolesMapDTO);
                userToAttendanceRolesMapDTO = userToAttendanceRolesMapDataHandler.Update(userToAttendanceRolesMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(userToAttendanceRolesMapDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("user_roles", userToAttendanceRolesMapDTO.Guid, sqlTransaction);
                }
                userToAttendanceRolesMapDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the UserToAttendanceRolesMapDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            if (userToAttendanceRolesMapDTO.IsActive)
            {
                /// Added Server Time
                LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                DateTime currentTime = serverTimeObject.GetServerDateTime();
            // Required validations to be added here
            if (userToAttendanceRolesMapDTO.UserToAttendanceRolesMapId != -1 && userToAttendanceRolesMapDTO.IsActive && DateTime.Compare(userToAttendanceRolesMapDTO.EffectiveDate, currentTime) < 0)
            {
                log.Debug("Please enter a valid Effective Date");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2836, MessageContainerList.GetMessage(executionContext, ""), MessageContainerList.GetMessage(executionContext, "Validation Error")));
            }
            if (userToAttendanceRolesMapDTO.EndDate != null && userToAttendanceRolesMapDTO.EffectiveDate >= userToAttendanceRolesMapDTO.EndDate)
            {
                log.Debug("Please enter a valid End Date");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2837, MessageContainerList.GetMessage(executionContext, ""), MessageContainerList.GetMessage(executionContext, "Validation Error")));
            }
            if (userToAttendanceRolesMapDTO.AttendanceRoleId < 0)
            {
                log.Debug("Please enter a valid Attendance Role Id");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2840, MessageContainerList.GetMessage(executionContext, ""), MessageContainerList.GetMessage(executionContext, "Validation Error")));
            }
            List<KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>> userToAttendanceRolesMapSearchParameters = new List<KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>>();
            userToAttendanceRolesMapSearchParameters.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.USER_ID, userToAttendanceRolesMapDTO.UserId.ToString()));
            userToAttendanceRolesMapSearchParameters.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.ATTENDANCE_ROLE_ID, userToAttendanceRolesMapDTO.AttendanceRoleId.ToString()));
            userToAttendanceRolesMapSearchParameters.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.IS_ACTIVE, "1"));
            userToAttendanceRolesMapSearchParameters.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            UserToAttendanceRolesMapDataHandler userToAttendanceRolesMapDataHandler = new UserToAttendanceRolesMapDataHandler(sqlTransaction);
            List<UserToAttendanceRolesMapDTO> userToAttendanceRolesMapDTOList = userToAttendanceRolesMapDataHandler.GetUserToAttendanceRolesMapDTOList(userToAttendanceRolesMapSearchParameters);
            if (userToAttendanceRolesMapDTOList != null && userToAttendanceRolesMapDTOList.Any())
                {
                    List<UserToAttendanceRolesMapDTO> orderedUserToAttendanceRolesMapDTOList = userToAttendanceRolesMapDTOList.OrderBy(pay => pay.EffectiveDate).ToList();
                    foreach (UserToAttendanceRolesMapDTO userToAttendanceRolesMapDTOObj in orderedUserToAttendanceRolesMapDTOList)
                    {
                        if (IncludesEffectiveDate(userToAttendanceRolesMapDTOObj, userToAttendanceRolesMapDTO))
                        {
                            log.Debug("Please enter a valid Effective Date");
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2836, MessageContainerList.GetMessage(executionContext, ""), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                        }

                        if (IncludesEndDate(userToAttendanceRolesMapDTOObj, userToAttendanceRolesMapDTO))
                        {
                            log.Debug("Please enter a valid End Date");
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2837, MessageContainerList.GetMessage(executionContext, ""), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                        }
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        public bool IncludesEffectiveDate(UserToAttendanceRolesMapDTO userToAttendanceRolesMapDTOObj, UserToAttendanceRolesMapDTO userToAttendanceRolesMapDTO)
        {
            return (userToAttendanceRolesMapDTO.EffectiveDate.Date >= userToAttendanceRolesMapDTOObj.EffectiveDate.Date && userToAttendanceRolesMapDTO.EffectiveDate < userToAttendanceRolesMapDTOObj.EndDate);
        }

        public bool IncludesEndDate(UserToAttendanceRolesMapDTO userToAttendanceRolesMapDTOObj, UserToAttendanceRolesMapDTO userToAttendanceRolesMapDTO)
        {
            return (userToAttendanceRolesMapDTO.EndDate >= userToAttendanceRolesMapDTOObj.EffectiveDate.Date && userToAttendanceRolesMapDTO.EndDate < userToAttendanceRolesMapDTOObj.EndDate)
                || (userToAttendanceRolesMapDTOObj.EndDate >= userToAttendanceRolesMapDTO.EffectiveDate.Date && userToAttendanceRolesMapDTOObj.EndDate < userToAttendanceRolesMapDTO.EndDate);
        }
         /// <summary>
         /// Gets the DTO
         /// </summary>
        public UserToAttendanceRolesMapDTO UserToAttendanceRolesMapDTO
        {
            get
            {
                return userToAttendanceRolesMapDTO;
            }
        }

    }
    /// <summary>
    /// Manages the list of UserToAttendanceRolesMap
    /// </summary>
    public class UserToAttendanceRolesMapListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly List<UserToAttendanceRolesMapDTO> userToAttendanceRolesMapDTOList = new List<UserToAttendanceRolesMapDTO>();

        /// <summary>
        /// Parameterized constructor for UserToAttendanceRolesMapListBL
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public UserToAttendanceRolesMapListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for UserToAttendanceRolesMapListBL
        /// </summary>
        /// <param name="executionContext">executionContext object passed as a parameter</param>
        /// <param name="userToAttendanceRolesMapDTOList">UserToAttendanceRolesMapDTOList passed as a parameter</param>
        public UserToAttendanceRolesMapListBL(ExecutionContext executionContext, List<UserToAttendanceRolesMapDTO> userToAttendanceRolesMapDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, userToAttendanceRolesMapDTOList);
            this.userToAttendanceRolesMapDTOList = userToAttendanceRolesMapDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the UserToAttendanceRolesMapDTOList based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returns the UserToAttendanceRolesMapDTO List</returns>
        public List<UserToAttendanceRolesMapDTO> GetUserToAttendanceRolesMapDTOList(List<KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>> searchParameters,
                                                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            UserToAttendanceRolesMapDataHandler userToAttendanceRolesMapDTODataHandler = new UserToAttendanceRolesMapDataHandler(sqlTransaction);
            List<UserToAttendanceRolesMapDTO> userToAttendanceRolesMapDTOList = userToAttendanceRolesMapDTODataHandler.GetUserToAttendanceRolesMapDTOList(searchParameters);
            log.LogMethodExit(userToAttendanceRolesMapDTOList);
            return userToAttendanceRolesMapDTOList;
        }

        /// <summary>
        /// Saves the UserToAttendanceRolesMapDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<UserToAttendanceRolesMapDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<UserToAttendanceRolesMapDTO> userToAttendanceRolesMapDTOLists = new List<UserToAttendanceRolesMapDTO>();
            if (userToAttendanceRolesMapDTOList != null)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (UserToAttendanceRolesMapDTO userToAttendanceRolesMapDTO in userToAttendanceRolesMapDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            UserToAttendanceRolesMapBL userToAttendanceRolesMapBL = new UserToAttendanceRolesMapBL(executionContext, userToAttendanceRolesMapDTO);
                            userToAttendanceRolesMapBL.Save(sqlTransaction);
                            userToAttendanceRolesMapDTOLists.Add(userToAttendanceRolesMapBL.UserToAttendanceRolesMapDTO);
                            parafaitDBTrx.EndTransaction();

                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit();
                }
            }
            return userToAttendanceRolesMapDTOLists;
        }
    }
}
