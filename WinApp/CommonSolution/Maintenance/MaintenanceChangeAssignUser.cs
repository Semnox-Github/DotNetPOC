/********************************************************************************************
 * Project Name - MaintenanceChangeAssignUser
 * Description  - Bussiness logic of Maintenance Change Assign User
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00       23-Nov-2016    Raghuveera     created 
 *2.70       08-Jul-2019    Dakshakh raj   Modified (Added sqlTransaction)
 *2.70       12-Mar-2019    Guru S A       Modified for schedule class renaming as par of booking phase2 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// The class which saves the data back to the data base.
    /// </summary>
    public class MaintenanceChangeAssignUser
    {
        private UserJobItemsDTO userJobItemsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public MaintenanceChangeAssignUser(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            userJobItemsDTO = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="userJobItemsId"></param>
        public MaintenanceChangeAssignUser(ExecutionContext executionContext, int userJobItemsId)
        {
            log.LogMethodEntry(executionContext, userJobItemsId);
            this.executionContext = executionContext;
            UserJobItemsDatahandler userJobItemsDatahandler = new UserJobItemsDatahandler(null);
            this.userJobItemsDTO = userJobItemsDatahandler.GetUserJobItemsDTO(userJobItemsId);
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="userJobItemsDTO"></param>
        public MaintenanceChangeAssignUser(ExecutionContext executionContext, UserJobItemsDTO userJobItemsDTO)
        {
            log.LogMethodEntry(executionContext, userJobItemsDTO);
            this.executionContext = executionContext;
            this.userJobItemsDTO = userJobItemsDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the maintenance job
        /// Job will be inserted if MaintChklstdetId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId);
            UserJobItemsDatahandler userJobItemDataHandler = new UserJobItemsDatahandler(null);
            if (userJobItemsDTO.MaintChklstdetId < 0)
            {
                int maintenanceJobId = userJobItemDataHandler.InsertUserJobItems(userJobItemsDTO, executionContext.GetUserId(), siteId);
                userJobItemsDTO.MaintChklstdetId = maintenanceJobId;
            }
            else
            {
                if (userJobItemsDTO.IsChanged == true)
                {
                    userJobItemDataHandler.UpdateUserJobItems(userJobItemsDTO, executionContext.GetUserId(), siteId);
                    userJobItemsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }
    /// <summary>
    /// Manages the list of maintenance Job which are published from HQ
    /// </summary>
    public class MaintenanceChangeAssignJobList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public MaintenanceChangeAssignJobList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// The jobs which are published from the hq are fetched using this method
        /// </summary>
        /// <param name="searchParameters"> The search parameter are sent based on the filter option selected. </param>
        /// <returns>Returns the list of job.</returns>
        public List<UserJobItemsDTO> GetAllMaintenanceWithHQPublishedJobList(List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            UserJobItemsDatahandler userJobItemDatahandler = new UserJobItemsDatahandler(null);
            List<UserJobItemsDTO> userJobItemsDTOList = userJobItemDatahandler.GetAllUserJobItemsWithHQPublishedJobList(searchParameters);
            log.LogMethodExit(userJobItemsDTOList);
            return userJobItemsDTOList;
        }

        
    }
}
