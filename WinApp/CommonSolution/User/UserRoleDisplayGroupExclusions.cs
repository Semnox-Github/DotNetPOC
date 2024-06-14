
/********************************************************************************************
 * Project Name - DisplayGroup
 * Description  - Bussiness logic of the UserRole DisplayGroups class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        18-may-2016   Amaresh          Created 
 *2.3.0       25-Jun-2018   Guru S A         Rename the class as per db object modifications
 *                                           For User role level product exclusion change 
 *2.70        17-Mar-2016   Jagan Mohana     Modified - Added SaveUpdateUserRoleDisplayGroupExclusionsList() & Constructor
 *            25-Jun-2019   Mushahid Faizan  Added log Method Entry & Exit & modified SaveUpdateUserRoleDisplayGroupExclusionsList().
 *                                           Added Delete for Hard-Deletion in SiteSetup
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    ///  UserRoleDisplayGroupExclusions 
    /// </summary>

    public class UserRoleDisplayGroupExclusions
    {
        UserRoleDisplayGroupExclusionsDTO userRoleDisplayGroupExclusions;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor of UserRoleDisplayGroupExclusions class having ExecutionContext
        /// </summary>
        private UserRoleDisplayGroupExclusions(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the roleDisplayGroupId id as the parameter
        /// Would fetch the UserRoleDisplayGroupExclusionsDTO object from the database based on the roleDisplayGroupId passed. 
        /// </summary>
        /// <param name="roleDisplayGroupId">Display Group</param>
        public UserRoleDisplayGroupExclusions(ExecutionContext executionContext, int roleDisplayGroupId)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, roleDisplayGroupId);
            UserRoleDisplayGroupExclusionsDataHandler userRoleDisplayGroupExclusionsDataHandler = new UserRoleDisplayGroupExclusionsDataHandler();
            userRoleDisplayGroupExclusions = userRoleDisplayGroupExclusionsDataHandler.GetUserRoleDisplayGroupExclusions(roleDisplayGroupId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates userRoleDisplayGroupExclusions object using the UserRoleDisplayGroupExclusionsDTO
        /// </summary>
        /// <param name="userRoleDisplayGroupExclusions">UserRoleDisplayGroupExclusionsDTO object</param>
        public UserRoleDisplayGroupExclusions(ExecutionContext executionContext, UserRoleDisplayGroupExclusionsDTO userRoleDisplayGroupExclusions)
            : this(executionContext)
        {
            log.LogMethodEntry();
            this.userRoleDisplayGroupExclusions = userRoleDisplayGroupExclusions;
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the userRoleDisplayGroupExclusions 
        /// Checks if the RoleDisplayGroupId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            UserRoleDisplayGroupExclusionsDataHandler userRoleDisplayGroupExclusionsDataHandler = new UserRoleDisplayGroupExclusionsDataHandler(sqlTransaction);
            if (userRoleDisplayGroupExclusions.IsActive)
            {

                if (userRoleDisplayGroupExclusions.RoleDisplayGroupId <= 0)
                {
                    userRoleDisplayGroupExclusions = userRoleDisplayGroupExclusionsDataHandler.InsertUserRoleDisplayGroupExclusions(userRoleDisplayGroupExclusions, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(userRoleDisplayGroupExclusions.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("UserRoleDisplayGroupExclusions", userRoleDisplayGroupExclusions.Guid, sqlTransaction);
                    }
                    userRoleDisplayGroupExclusions.AcceptChanges();
                }
                else
                {
                    if (userRoleDisplayGroupExclusions.IsChanged)
                    {
                        userRoleDisplayGroupExclusions = userRoleDisplayGroupExclusionsDataHandler.UpdateUserRoleDisplayGroupExclusions(userRoleDisplayGroupExclusions, executionContext.GetUserId(), executionContext.GetSiteId());
                        if (!string.IsNullOrEmpty(userRoleDisplayGroupExclusions.Guid))
                        {
                            AuditLog auditLog = new AuditLog(executionContext);
                            auditLog.AuditTable("UserRoleDisplayGroupExclusions", userRoleDisplayGroupExclusions.Guid, sqlTransaction);
                        }
                        userRoleDisplayGroupExclusions.AcceptChanges();
                    }
                }
            }
            else
            {
                if (userRoleDisplayGroupExclusions.RoleDisplayGroupId >= 0)
                {
                    userRoleDisplayGroupExclusionsDataHandler.Delete(userRoleDisplayGroupExclusions.RoleDisplayGroupId);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public UserRoleDisplayGroupExclusionsDTO GetUserRoleDisplayGroupExclusions
        {
            get { return userRoleDisplayGroupExclusions; }
        }
    }

    /// <summary>
    /// Manages the list of UserRole DisplayGroups
    /// </summary>
    public class UserRoleDisplayGroupExclusionsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<UserRoleDisplayGroupExclusionsDTO> userRoleDisplayGroupExclusionsDTOsList;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UserRoleDisplayGroupExclusionsList()
        {
            log.LogMethodEntry();
        }

        /// <summary>
        /// Parameterized constructor having executionContext.
        /// </summary>
        public UserRoleDisplayGroupExclusionsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parametarized constructor
        /// </summary>
        /// <param name="userRoleDisplayGroupExclusionsDTOsList"></param>
        /// <param name="executionContext"></param>
        public UserRoleDisplayGroupExclusionsList(ExecutionContext executionContext,
                   List<UserRoleDisplayGroupExclusionsDTO> userRoleDisplayGroupExclusionsDTOsList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, userRoleDisplayGroupExclusionsDTOsList);
            this.userRoleDisplayGroupExclusionsDTOsList = userRoleDisplayGroupExclusionsDTOsList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Product Display GroupList
        /// </summary>
        public List<UserRoleDisplayGroupExclusionsDTO> GetAllUserRoleDisplayGroupExclusionsList(List<KeyValuePair<UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            UserRoleDisplayGroupExclusionsDataHandler userRoleDisplayGroupExclusionsDataHandler = new UserRoleDisplayGroupExclusionsDataHandler();
            List<UserRoleDisplayGroupExclusionsDTO> userRoleDisplayGroupExclusionsDTOs=  userRoleDisplayGroupExclusionsDataHandler.GetUserRoleDisplayGroupExclusionsList(searchParameters);
            log.LogMethodExit(userRoleDisplayGroupExclusionsDTOs);
            return userRoleDisplayGroupExclusionsDTOs;
        }

        /// <summary>
        /// This method is used to Save and Update the userRoleDisplayGroupExclusions details for Web Management Studio.
        /// </summary>
        public void Save(SqlTransaction sqlTransaction =null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (userRoleDisplayGroupExclusionsDTOsList != null && userRoleDisplayGroupExclusionsDTOsList.Count != 0)
                {
                    foreach (UserRoleDisplayGroupExclusionsDTO userRoleDisplayGroupExclusionsDto in userRoleDisplayGroupExclusionsDTOsList)
                    {
                        UserRoleDisplayGroupExclusions userRoleDisplayGroupExclusions = new UserRoleDisplayGroupExclusions(executionContext, userRoleDisplayGroupExclusionsDto);
                        userRoleDisplayGroupExclusions.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit();
        }

    }
}
