/********************************************************************************************
 * Project Name - ManagementFormAccess BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
  *2.70       25-Jun-2019   Mushahid Faizan         Added SaveUpdateManagementFormAccessList() and Constructor in List Class.
  *2.70.2       15-Jul-2019      Girish Kundar        Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public class ManagementFormAccessBL
    {
        private ManagementFormAccessDTO managementFormAccessDTO;
        private readonly ExecutionContext executionContext;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of ManagementFormAccessBL class
        /// </summary>
        private ManagementFormAccessBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the managementFormAccessBL id as the parameter
        /// Would fetch the managementFormAccessBL object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public ManagementFormAccessBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ManagementFormAccessDataHandler managementFormAccessBLDataHandler = new ManagementFormAccessDataHandler(sqlTransaction);
            managementFormAccessDTO = managementFormAccessBLDataHandler.GetManagementFormAccessDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ManagementFormAccessBL object using the ManagementFormAccessDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="managementFormAccessDTO">ManagementFormAccessDTO object</param>
        public ManagementFormAccessBL(ExecutionContext executionContext, ManagementFormAccessDTO managementFormAccessDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, managementFormAccessDTO);
            this.managementFormAccessDTO = managementFormAccessDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ManagementFormAccess
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ManagementFormAccessDataHandler managementFormAccessBLDataHandler = new ManagementFormAccessDataHandler(sqlTransaction);
            if (managementFormAccessDTO.ManagementFormAccessId < 0)
            {
                managementFormAccessDTO = managementFormAccessBLDataHandler.InsertManagementFormAccess(managementFormAccessDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                managementFormAccessDTO.AcceptChanges();
            }
            else
            {
                if (managementFormAccessDTO.IsChanged)
                {
                    managementFormAccessDTO = managementFormAccessBLDataHandler.UpdateManagementFormAccess(managementFormAccessDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    managementFormAccessDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ManagementFormAccessDTO ManagementFormAccessDTO
        {
            get
            {
                return managementFormAccessDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of ManagementFormAccess
    /// </summary>
    public class ManagementFormAccessListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        List<ManagementFormAccessDTO> managementFormAccessList;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public ManagementFormAccessListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ManagementFormAccessListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.managementFormAccessList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="managementFormAccessList">managementFormAccessList</param>
        public ManagementFormAccessListBL(ExecutionContext executionContext, List<ManagementFormAccessDTO> managementFormAccessList)
        {
            log.LogMethodEntry(executionContext, managementFormAccessList);
            this.executionContext = executionContext;
            this.managementFormAccessList = managementFormAccessList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the ManagementFormAccess list
        /// </summary>
        public List<ManagementFormAccessDTO> GetManagementFormAccessDTOList(List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ManagementFormAccessDataHandler managementFormAccessBLDataHandler = new ManagementFormAccessDataHandler(sqlTransaction);
            List<ManagementFormAccessDTO> returnValue = managementFormAccessBLDataHandler.GetManagementFormAccessDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// This method is used to Save and Update the Management Form Access details for web management studio.
        /// </summary>
        public List<ManagementFormAccessDTO> SaveUpdateManagementFormAccessList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (managementFormAccessList == null ||
                managementFormAccessList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception("Can't save empty list.");
            }

            ManagementFormAccessDataHandler managementFormAccessDataHandler = new ManagementFormAccessDataHandler(sqlTransaction);
            managementFormAccessDataHandler.Save(managementFormAccessList, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
            return managementFormAccessList;
        }

        /// <summary>
        /// HasMgmtFormAccess
        /// Checks whether user has access to the management form access params passed
        /// </summary>
        public bool HasMgmtFormAccess(string groupName, string menuName, string entryName, int userRoleId, string entryGuid = null)
        {
            log.LogMethodEntry(groupName, menuName, entryName, entryGuid);
            bool retValue = false;
            if (userRoleId != -1)
            {
                List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.FUNCTION_GROUP, groupName));
                searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.MAIN_MENU, menuName));
                searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.FORM_NAME, entryName));
                searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ROLE_ID, userRoleId.ToString()));
                searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ISACTIVE, "1"));
                if (entryGuid != null)
                    searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.FUNCTION_GUID, entryGuid));

                List<ManagementFormAccessDTO> managementFormAccessList = GetManagementFormAccessDTOList(searchParams);
                if (managementFormAccessList != null && managementFormAccessList.Count > 0)
                {
                    retValue = managementFormAccessList[0].AccessAllowed;
                }
            }
            log.LogMethodExit(retValue);
            return retValue;
        }
    }
}
