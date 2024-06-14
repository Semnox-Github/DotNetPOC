
/********************************************************************************************
 * Project Name - UserRolePriceList
 * Description  - Bussiness logic of the  UserRole PriceList class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        18-may-2016   Amaresh          Created 
 *2.60        25-Mar-2019   Jagan Mohana Rao     added constructors and SaveUpdateUserRolePriceList() method in UserRolePriceListBL
              25-Mar-2019   Akshay Gulaganji    added author information and log.MethodEntry() and log.MethodExit() 
 *2.70        25-Jun-2019   Mushahid Faizan    Added log Method Entry & Exit & modified SaveUpdateUserRolePriceList() and Constructor.
 *2.70.2        17-Jul-2019   Deeksha            Modifications as per three tier standard.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.PriceList
{
    /// <summary>
    /// UserRole PriceList
    /// </summary>

    public class UserRolePriceList
    {
        private UserRolePriceListDTO userRolePriceListDTO;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Default constructor of ConcurrentPrograms class
        /// </summary>
        public UserRolePriceList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.userRolePriceListDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the rolePriceListId as the parameter
        /// Would fetch the UserRolePriceListDTO object from the database based on the RolePriceListId id passed. 
        /// </summary>
        /// <param name="rolePriceListId">PriceList id </param>
        public UserRolePriceList(ExecutionContext executionContext, int rolePriceListId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, rolePriceListId, sqlTransaction);
            UserRolePriceListDataHandler userRolePriceListDataHandler = new UserRolePriceListDataHandler(sqlTransaction);
            userRolePriceListDTO = userRolePriceListDataHandler.GetUserRolePriceList(rolePriceListId);
            if (userRolePriceListDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "userRolePriceList", rolePriceListId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="userRolePriceList">UserRolePriceListDTO object</param>
        /// <param name="executionContext">ExecutionContext object</param>
        public UserRolePriceList(ExecutionContext executionContext, UserRolePriceListDTO userRolePriceList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, userRolePriceList);
            this.userRolePriceListDTO = userRolePriceList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public UserRolePriceListDTO UserRolePriceListDTO
        {
            get
            {
                return userRolePriceListDTO;
            }
        }

        /// <summary>
        /// Saves the UserRole PriceList 
        /// Checks if the RolePriceListId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            UserRolePriceListDataHandler userRolePriceListDataHandler = new UserRolePriceListDataHandler(sqlTransaction);
            if (userRolePriceListDTO.IsActive)
            {
                if (userRolePriceListDTO.RolePriceListId <= 0)
                {
                    userRolePriceListDTO = userRolePriceListDataHandler.InsertuserRolePriceList(userRolePriceListDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(userRolePriceListDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("UserRolePriceList", userRolePriceListDTO.Guid, sqlTransaction);
                    }
                    userRolePriceListDTO.AcceptChanges();
                }
                else
                {
                    if (userRolePriceListDTO.IsChanged)
                    {
                        userRolePriceListDTO = userRolePriceListDataHandler.UpdateUserRolePriceList(userRolePriceListDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        if (!string.IsNullOrEmpty(userRolePriceListDTO.Guid))
                        {
                            AuditLog auditLog = new AuditLog(executionContext);
                            auditLog.AuditTable("UserRolePriceList", userRolePriceListDTO.Guid, sqlTransaction);
                        }
                        userRolePriceListDTO.AcceptChanges();
                    }
                }
            }
            else
            {
                if (userRolePriceListDTO.RolePriceListId >= 0)
                {
                    int id = userRolePriceListDataHandler.Delete(userRolePriceListDTO.RolePriceListId);
                    log.LogMethodExit(id);
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the DTO
        /// </summary>
        public UserRolePriceListDTO GetUserRolePriceList { get { return userRolePriceListDTO; } }
    }
    /// <summary>
    /// Manages the UserRole Price list of Price List
    /// </summary>
    public class UserRolePriceListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<UserRolePriceListDTO> userRolePriceListDTOsList;

        /// <summary>
        /// Parametarized constructor having ExecutionContext
        /// </summary>
        public UserRolePriceListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parametarized constructor having ExecutionContext
        /// </summary>
        public UserRolePriceListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parametarized constructor
        /// </summary>
        /// <param name="userRolePriceListDTOsList"></param>
        /// <param name="executionContext"></param>
        public UserRolePriceListBL(ExecutionContext executionContext, List<UserRolePriceListDTO> userRolePriceListDTOsList)
        {
            log.LogMethodEntry(executionContext, userRolePriceListDTOsList);
            this.userRolePriceListDTOsList = userRolePriceListDTOsList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the UserRole PriceList list
        /// </summary>
        public List<UserRolePriceListDTO> GetAllUserRolePriceList(List<KeyValuePair<UserRolePriceListDTO.SearchByUserRolePriceListParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            UserRolePriceListDataHandler userRolePriceListDataHandler = new UserRolePriceListDataHandler(sqlTransaction);
            List<UserRolePriceListDTO> userRolePriceListDTOList =  userRolePriceListDataHandler.GetUserRolePriceList(searchParameters, sqlTransaction);
            log.LogMethodExit(userRolePriceListDTOList);
            return userRolePriceListDTOList;
        }

        /// <summary>
        /// This method is used to Save and Update the userRolePriceListDTOsList details for web management studio.
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (userRolePriceListDTOsList != null && userRolePriceListDTOsList.Any())
                {
                    foreach (UserRolePriceListDTO userRolePriceListDTO in userRolePriceListDTOsList)
                    {
                        UserRolePriceList userRolePriceList = new UserRolePriceList(executionContext, userRolePriceListDTO);
                        userRolePriceList.Save(sqlTransaction);
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
