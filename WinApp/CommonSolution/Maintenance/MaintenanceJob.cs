/********************************************************************************************
 * Project Name - Maintenance Job
 * Description  - Bussiness logic of maintenance job
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00       19-Jan-2016    Raghuveera     Created 
 *2.70       07-Jul-2019    Dakshakh raj   Modified 
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Maintenance job will creates and modifies the jobs    
    /// </summary>

    public class MaintenanceJob
    {
        private MaintenanceJobDTO maintenanceJobDTO;
       private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Default constructor
        /// </summary>

        public MaintenanceJob()
        {
            log.LogMethodEntry();
            maintenanceJobDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="maintenanceJobDTO">Parameter of the type MaintenanceJobDTO</param>

        public MaintenanceJob(MaintenanceJobDTO maintenanceJobDTO)
        {
            log.LogMethodEntry(maintenanceJobDTO);
            this.maintenanceJobDTO = maintenanceJobDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the maintenance job
        /// Job will be inserted if MaintChklstdetId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            ExecutionContext jobUserContext = ExecutionContext.GetExecutionContext();
            MaintenanceJobDataHandler maintenanceJobDataHandler = new MaintenanceJobDataHandler(sqlTransaction);
            if (maintenanceJobDTO.MaintChklstdetId < 0)
            {
                maintenanceJobDTO = maintenanceJobDataHandler.InsertMaintenanceJob(maintenanceJobDTO, jobUserContext.GetUserId(), jobUserContext.GetSiteId());
                maintenanceJobDTO.AcceptChanges();
            }
            else
            {
                if (maintenanceJobDTO.IsChanged == true)
                {
                    maintenanceJobDTO = maintenanceJobDataHandler.UpdateMaintenanceJob(maintenanceJobDTO, jobUserContext.GetUserId(), jobUserContext.GetSiteId());
                    maintenanceJobDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of Maintenance Job
    /// </summary>

    public class MaintenanceJobList
    {
        public static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Fetches the job matching with job id
        /// </summary>
        /// <param name="Jobid">Id of the job</param>
        /// <returns>MaintenanceJobDTO object</returns>

        public MaintenanceJobDTO GetMaintenanceJob(int Jobid,SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(Jobid);
            MaintenanceJobDataHandler maintenanceJobDataHandler = new MaintenanceJobDataHandler(sqlTransaction);
            log.LogMethodExit(maintenanceJobDataHandler.GetMaintenanceJob(Jobid));
            return maintenanceJobDataHandler.GetMaintenanceJob(Jobid);
        }

        /// <summary>
        /// Gets All MaintenanceJobs list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="userId"></param>
        /// <returns>Returns the maintenance job list</returns>

        public List<MaintenanceJobDTO> GetAllMaintenanceJobs(List<KeyValuePair<MaintenanceJobDTO.SearchByMaintenanceJobParameters, string>> searchParameters, int userId,SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(searchParameters);
            MaintenanceJobDataHandler maintenanceJobDataHandler = new MaintenanceJobDataHandler(sqlTransaction);            
            if (userId == -1)
            {
                log.LogMethodExit(maintenanceJobDataHandler.GetAllMaintenanceJobList(searchParameters));
                    return maintenanceJobDataHandler.GetAllMaintenanceJobList(searchParameters);
            }
            else
            {
                log.LogMethodExit(maintenanceJobDataHandler.GetMaintenanceJobList(searchParameters, userId));
                    return maintenanceJobDataHandler.GetMaintenanceJobList(searchParameters, userId);
            }
        }

        /// <summary>
        /// Returns the job list in batch. Used for external systems.
        /// <param name="searchParameters">Parameter list for the select query</param>
        /// <param name="maxRows">Maximum Rows to be returned</param>
        /// </summary>

        public List<MaintenanceJobDTO> GetAllMaintenanceJobsBatch(List<KeyValuePair<MaintenanceJobDTO.SearchByMaintenanceJobParameters, string>> searchParameters, int maxRows = int.MaxValue,SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(searchParameters);
            MaintenanceJobDataHandler maintenanceJobDataHandler = new MaintenanceJobDataHandler(sqlTransaction);
            log.LogMethodExit(maintenanceJobDataHandler.GetMaintenanceJobListBatch(searchParameters, maxRows));
            return maintenanceJobDataHandler.GetMaintenanceJobListBatch(searchParameters, maxRows);
        }
    }
}
