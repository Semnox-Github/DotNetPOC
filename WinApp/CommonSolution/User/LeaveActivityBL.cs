/********************************************************************************************
 * Project Name - User - LeaveActivity
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.80        20-May-2020      Vikas Dwivedi       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public class LeaveActivityBL
    {
        private LeaveActivityDTO leaveActivityDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor of LeaveActivityBL
        /// </summary>
        private LeaveActivityBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates LeaveActivityBL object using the leaveActivityDTO
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="leaveActivityDTO">leaveActivityDTO</param>
        public LeaveActivityBL(ExecutionContext executionContext, LeaveActivityDTO leaveActivityDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, leaveActivityDTO);
            this.leaveActivityDTO = leaveActivityDTO;
            log.LogMethodExit();
        }
    }

    public class LeaveActivityListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<LeaveActivityDTO> leaveActivityDTOList = new List<LeaveActivityDTO>();

        /// <summary>
        /// Parameterized Constructor of LeaveActivityListBL
        /// </summary>
        /// <param name="executionContext"></param>
        public LeaveActivityListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public LeaveActivityListBL(ExecutionContext executionContext, List<LeaveActivityDTO> leaveActivityDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, leaveActivityDTOList);
            this.leaveActivityDTOList = leaveActivityDTOList;
            log.LogMethodExit();
        }

        public LeaveActivityDTO GetLeaveActivities(int userId = -1)
        {
            UsersList usersList = new UsersList(executionContext);
            UsersDTO user = new UsersDTO();
            LeaveActivityDTO leaveActivityDTOs = new LeaveActivityDTO();
            // loadRecords for UserDTO
            if (userId > -1)
            {
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchByUsersParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchByUsersParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchByUsersParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_ID, userId.ToString()));
                leaveActivityDTOs.UsersDTOList = usersList.GetAllUsers(searchByUsersParameters);
                user = leaveActivityDTOs.UsersDTOList.Find(x => x.UserId == userId);

            }
            else
            {
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchByUsersParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchByUsersParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchByUsersParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, executionContext.GetUserId()));
                leaveActivityDTOs.UsersDTOList = usersList.GetAllUsers(searchByUsersParameters);
                user = leaveActivityDTOs.UsersDTOList.Find(x => x.LoginId == executionContext.GetUserId());
            }
            // loadRecords for UserRolesDTO
            List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> userRolesSearchParameters = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
            userRolesSearchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            userRolesSearchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.ROLE_ID, user.RoleId.ToString()));
            UserRolesList userRolesList = new UserRolesList(executionContext);
            leaveActivityDTOs.UserRolesDTOList = userRolesList.GetAllUserRoles(userRolesSearchParameters);

            //loadRecords for DepartmentDTO
            List<KeyValuePair<DepartmentDTO.SearchByParameters, string>> departmentSearchParameters = new List<KeyValuePair<DepartmentDTO.SearchByParameters, string>>();
            departmentSearchParameters.Add(new KeyValuePair<DepartmentDTO.SearchByParameters, string>(DepartmentDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            departmentSearchParameters.Add(new KeyValuePair<DepartmentDTO.SearchByParameters, string>(DepartmentDTO.SearchByParameters.DEPARTMENT_ID, user.DepartmentId.ToString()));
            DepartmentList departmentList = new DepartmentList(executionContext);
            leaveActivityDTOs.DepartmentDTOList = departmentList.GetDepartmentDTOList(departmentSearchParameters);

            //loadRecords for LeaveDTO
            List<KeyValuePair<LeaveDTO.SearchByParameters, string>> searchByLeaveParameters = new List<KeyValuePair<LeaveDTO.SearchByParameters, string>>();
            searchByLeaveParameters.Add(new KeyValuePair<LeaveDTO.SearchByParameters, string>(LeaveDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            LeaveListBL leaveListBL = new LeaveListBL(executionContext);
            leaveActivityDTOs.LeaveDTOList = leaveListBL.GetLeaveDTOList(searchByLeaveParameters);
            if (user.UserId != -1)
            {
                leaveActivityDTOs.LeaveTypeBalanceDTOList = leaveListBL.LoadLeaveBalances(user.UserId);
            }
            return leaveActivityDTOs;
        }
    }
}
