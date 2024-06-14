
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
 *2.110.0     03-Dec-2020   Prajwal S        Modified Three tier
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    /// <summary>
    ///  UserRoleDisplayGroups 
    /// </summary>

    public class UserRoleDisplayGroups
    {

        private UserRoleDisplayGroupsDTO userRoleDisplayGroupsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of UserRoleDisplayGroups class
        /// </summary>
        private UserRoleDisplayGroups(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates UserRoleDisplayGroupsBL object using the UserRoleDisplayGroupsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="userRoleDisplayGroupsDTO">UserRoleDisplayGroupsDTO DTO object</param>
        public UserRoleDisplayGroups(ExecutionContext executionContext, UserRoleDisplayGroupsDTO userRoleDisplayGroupsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext,userRoleDisplayGroupsDTO);
            this.userRoleDisplayGroupsDTO = userRoleDisplayGroupsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the UserRoleDisplayGroups  id as the parameter
        /// Would fetch the UserRoleDisplayGroups object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="roleDisplayGroupId">id -PromotionRule </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public UserRoleDisplayGroups(ExecutionContext executionContext, int userRoleDisplayGroupsId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, userRoleDisplayGroupsId, sqlTransaction);
            UserRoleDisplayGroupsDataHandler userRoleDisplayGroupsDataHandler = new UserRoleDisplayGroupsDataHandler(sqlTransaction);
            userRoleDisplayGroupsDTO = userRoleDisplayGroupsDataHandler.GetUserRoleDisplayGroups(userRoleDisplayGroupsId);
            if (userRoleDisplayGroupsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "UserRoleDisplayGroupsDTO", userRoleDisplayGroupsId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(userRoleDisplayGroupsDTO);
        }

        /// <summary>
        /// Saves the userRoleDisplayGroups 
        /// Checks if the RoleDisplayGroupId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (userRoleDisplayGroupsDTO.IsChanged == false
                && userRoleDisplayGroupsDTO.RoleDisplayGroupId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            UserRoleDisplayGroupsDataHandler userRoleDisplayGroupDataHandler = new UserRoleDisplayGroupsDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (userRoleDisplayGroupsDTO.RoleDisplayGroupId < 0)
            {
                userRoleDisplayGroupsDTO = userRoleDisplayGroupDataHandler.Insert(userRoleDisplayGroupsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                userRoleDisplayGroupsDTO.AcceptChanges();
            }
            else if (userRoleDisplayGroupsDTO.IsChanged)
            {
                userRoleDisplayGroupsDTO = userRoleDisplayGroupDataHandler.Update(userRoleDisplayGroupsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                userRoleDisplayGroupsDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (userRoleDisplayGroupsDTO == null)
            {
                //Validation to be implemented.
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public UserRoleDisplayGroupsDTO UserRoleDisplayGroupsDTO { get { return userRoleDisplayGroupsDTO; } }
    }

     /// <summary>
    /// Manages the list of UserRole DisplayGroups
    /// </summary>
    public class UserRoleDisplayGroupsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<UserRoleDisplayGroupsDTO> userRoleDisplayGroupsDTOList = new List<UserRoleDisplayGroupsDTO>();


        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public UserRoleDisplayGroupsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="userRoleDisplayGroupsDTOList">UserRoleDisplayGroup DTO List as parameter </param>
        public UserRoleDisplayGroupsList(ExecutionContext executionContext, List<UserRoleDisplayGroupsDTO> userRoleDisplayGroupsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, userRoleDisplayGroupsDTOList);
            this.userRoleDisplayGroupsDTOList = userRoleDisplayGroupsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Product Display GroupList
        /// </summary>
        public List<UserRoleDisplayGroupsDTO> GetAllUserRoleDisplayGroupsList(List<KeyValuePair<UserRoleDisplayGroupsDTO.SearchByDisplayGroupsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            UserRoleDisplayGroupsDataHandler userRoleDisplayGroupsDataHandler = new UserRoleDisplayGroupsDataHandler(sqlTransaction);
            List<UserRoleDisplayGroupsDTO> userRoleDisplayGroupsDTOList = userRoleDisplayGroupsDataHandler.GetUserRoleDisplayGroupsList(searchParameters, sqlTransaction);
            log.LogMethodExit(userRoleDisplayGroupsDTOList);
            return userRoleDisplayGroupsDTOList;
        }

        /// <summary>
        /// Saves the  list of UserRoleDisplayGroupsList DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (userRoleDisplayGroupsDTOList == null ||
                userRoleDisplayGroupsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < userRoleDisplayGroupsDTOList.Count; i++)
            {
                UserRoleDisplayGroupsDTO userRoleDisplayGroupsDTO = userRoleDisplayGroupsDTOList[i];
                if (userRoleDisplayGroupsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    UserRoleDisplayGroups userRoleDisplayGroups = new UserRoleDisplayGroups(executionContext, userRoleDisplayGroupsDTO);
                    userRoleDisplayGroups.Save(sqlTransaction);
                }
                catch (SqlException ex)
                {
                    log.Error(ex);
                    if (ex.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else if (ex.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving RecipeEstimationDetailsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("RecipeEstimationDetailsDTO", userRoleDisplayGroupsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }
}
