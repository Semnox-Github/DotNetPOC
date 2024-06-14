/********************************************************************************************
* Project Name - User
* Description  - Implementation of user use-cases 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.110.0     12-Nov-2019   Lakshminarayana         Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Implementation of user use-cases
    /// </summary>
    public class LocalUserUseCases : LocalUseCases, IUserUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LocalUserUseCases(ExecutionContext executionContext) : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<UsersDTO>> GetUserDTOList(List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters, bool loadChildRecords = false, bool activeChildRecords = true)
        {
            return await Task<List<UsersDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords);
                UsersList usersList = new UsersList(executionContext);
                List<UsersDTO> usersDTOList = usersList.GetAllUsers(searchParameters, loadChildRecords, activeChildRecords);
                if (usersDTOList != null && usersDTOList.Any())
                {
                    Users user = new Users(executionContext, executionContext.UserPKId);
                    UsersDTO userDTO = user.UserDTO;
                    UserRoles userRoles = new UserRoles(executionContext, userDTO.RoleId, true, true);
                    string roles = userRoles.GetFormAccessRoles(userDTO.RoleId);
                    string securityPolicy = string.Empty;
                    if (userRoles.getUserRolesDTO != null)
                    {
                        SecurityPolicyBL securityPolicyBL = new SecurityPolicyBL(executionContext, userRoles.getUserRolesDTO.SecurityPolicyId);
                        if (securityPolicyBL.getSecurityPolicyDTO != null)
                        {
                            securityPolicy = securityPolicyBL.getSecurityPolicyDTO.PolicyName;
                        }
                    }
                    if (!string.IsNullOrEmpty(roles))
                    {
                        usersDTOList = (from mm in usersDTOList
                                        where mm.IsActive == true &&
                     ((roles.Contains(mm.RoleId.ToString()) && mm.LoginId != "semnox" && "semnox" != executionContext.UserId)
                     || mm.LoginId == "semnox" && "semnox" == executionContext.UserId && "PA-DSS" == securityPolicy)
                     || ("semnox" == executionContext.UserId && "PA-DSS" != securityPolicy)
                                        orderby mm.UserId
                                        select mm).ToList();
                    }
                }
                log.LogMethodExit(usersDTOList);
                return usersDTOList;
            });
        }

        public async Task<List<UsersDTO>> SaveUsersDTOList(List<UsersDTO> usersDTOList)
        {
            return await Task<List<UsersDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    UsersList usersList = new UsersList(executionContext, usersDTOList);
                    List<UsersDTO> result = usersList.Save();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }

        public async Task<UserContainerDTOCollection> GetUserContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<UserContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    UserContainerList.Rebuild(siteId);
                }
                UserContainerDTOCollection result = UserContainerList.GetUserContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<UserIdentificationTagsDTO> UpdateUserIdentificationTagStatus(int userId, int tagId, UserIdentificationTagsDTO userIdentificationTagsDTO)
        {
            return await Task<UserIdentificationTagsDTO>.Factory.StartNew(() =>
            {
                UserIdentificationTags userIdentificationTagsBL = null;
                UserIdentificationTagsDTO result = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (tagId == -1)
                        {
                            throw new Exception(" tag Id is not found");
                        }
                        if (userId == -1)
                        {
                            throw new Exception(" user Id is not found");
                        }                     
                        userIdentificationTagsDTO.ActiveFlag = false;
                        userIdentificationTagsDTO.EndDate = DateTime.Now;
                        userIdentificationTagsBL = new UserIdentificationTags(executionContext, userIdentificationTagsDTO);
                        userIdentificationTagsBL.Save(parafaitDBTrx.SQLTrx);
                        result = userIdentificationTagsDTO;
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
                        throw ex;
                    }
                }
                log.LogMethodExit(userIdentificationTagsDTO);
                return userIdentificationTagsDTO;
            });
        }

        /// <summary>
        /// RecordAttendance
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="attendanceLogDTO"></param>
        /// <param name="sqlTransaction"></param>
        public async Task RecordAttendance(int userId, AttendanceLogDTO attendanceLogDTO, SqlTransaction sqlTransaction)
        {
            await Task.Factory.StartNew(() =>
            {
                log.LogMethodEntry(userId, attendanceLogDTO, sqlTransaction);
                Users users = new Users(executionContext, userId, true, true);
                users.RecordAttendance(-1, attendanceLogDTO.UserId, null, -1, ServerDateTime.Now, attendanceLogDTO.ReaderId, attendanceLogDTO.Mode,
                    attendanceLogDTO.TipValue, -1, null, null, null, null, attendanceLogDTO.CardNumber, null, false);
                log.LogMethodExit();
            });
        }

        public async Task<List<UserIdentificationTagsDTO>> GetUserIdentificationTagsDTOList(List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>> parameters)
        {
            return await Task<List<UserIdentificationTagsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                UserIdentificationTagListBL userIdentificationTagListBL = new UserIdentificationTagListBL(executionContext);
                List<UserIdentificationTagsDTO> userIdentificationTagList = userIdentificationTagListBL.GetUserIdentificationTagsDTOList(parameters);
                log.LogMethodExit(userIdentificationTagList);
                return userIdentificationTagList;
            });
        }

        public async Task<List<ManagementFormAccessContainerDTO>> GetUserManagementFormAccess()
        {
            return await Task<List<ManagementFormAccessContainerDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry();
                int siteId = executionContext.SiteId;
                int userPKId = executionContext.GetUserPKId();
                int roleId = -1;
                List<ManagementFormAccessContainerDTO> managementFormAccessContainerDTOList = null;
                if(userPKId != -1)
                {
                    roleId = UserContainerList.GetUserContainerDTO(siteId, userPKId).RoleId;
                    if (roleId != -1)
                    {
                        managementFormAccessContainerDTOList = UserRoleContainerList.GetUserRoleContainerDTO(siteId, roleId).ManagementFormAccessContainerDTOList;
                    }
                    else
                    {
                        string errorMessage = "Role Id " + roleId + " not found";
                        log.LogMethodExit(errorMessage);
                        throw new ValidationException(errorMessage);
                    }
                }
                else
                {
                    string errorMessage = "UserId Id " + roleId + " not found";
                    log.LogMethodExit(errorMessage);
                    throw new ValidationException(errorMessage);
                }
                
                log.LogMethodExit(managementFormAccessContainerDTOList);
                return managementFormAccessContainerDTOList;
            });
        }
    }
}
