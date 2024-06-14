/********************************************************************************************
 * Project Name - Maintenance Job summary
 * Description  - Logical grouping of maintenance job summary so that tasks can be assigned to the user
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        19-Jan-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
    public class MaintenanceJobSummary
    {
        /// <summary>
        /// Fetch the user with user id 
        /// </summary>
        /// <returns>Returns the user details as DataTable object </returns>
        public DataTable GetUser()
        {
            MaintenanceJobDataHandler maintenanceJobDataHandler = new MaintenanceJobDataHandler();
            return maintenanceJobDataHandler.getUser();
        }
        /// <summary>
        ///  Fetch the Status from lookup
        /// </summary>
        /// <param name="StatusName"> Is a defualt parameter to fetch perticular status</param>
        /// <returns>Returns the user details as DataTable object</returns>        
        public DataTable GetLookupStatus(string StatusName = "")
        {
            MaintenanceJobDataHandler maintenanceJobDataHandler = new MaintenanceJobDataHandler();
            return maintenanceJobDataHandler.getLookUpStatus(StatusName);
        }
        
    }
    /// <summary>
    /// Manages the list of Maintenance Job
    /// </summary>
    public class MaintenanceJobSummaryList
    {
        /// <summary>
        /// Returns the maintenance task list
        /// </summary>
        public List<MaintenanceJobSummaryDTO> GetAllMaintenanceJobSummarys(List<KeyValuePair<MaintenanceJobSummaryDTO.SearchByMaintenanceJobSummaryParameters, string>> searchParameters)
        {
            MaintenanceJobSummaryDataHandler maintenanceJobSummaryDataHandler = new MaintenanceJobSummaryDataHandler();
            return maintenanceJobSummaryDataHandler.GetMaintenanceJobSummaryList(searchParameters);
        }
    }
}
