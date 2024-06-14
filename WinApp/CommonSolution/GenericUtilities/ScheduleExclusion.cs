/********************************************************************************************
 * Project Name - Schedule Exclusion
 * Description  - Schedule Exclusion business logics 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        13-Jan-2016   Raghuveera          Created
 *2.40        29-Sep-2018   Jagan Mohan         Added new constructor ScheduleExclusionList and 
 *                                                 methods SaveUpdateScheduleExclusionsList
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Schedule Exclusion inserts and modifies the schedule inclusion/exclusion records
    /// </summary>
    public class ScheduleExclusion
    {
        private static readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ScheduleExclusionDTO scheduleExclusionDTO;
        private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor
        /// </summary>
        public ScheduleExclusion()
        {
            log.Debug("starts-ScheduleExclusion() Default constructor");
            scheduleExclusionDTO = null;
            this.executionContext = ExecutionContext.GetExecutionContext();
            log.Debug("Ends-ScheduleExclusion() Default constructor");
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="scheduleExclusionDTO">Schedule Exclusion DTO</param>
        public ScheduleExclusion(ScheduleExclusionDTO scheduleExclusionDTO)
        {
            log.LogMethodEntry(scheduleExclusionDTO);
            this.scheduleExclusionDTO = scheduleExclusionDTO;
            this.executionContext = ExecutionContext.GetExecutionContext();
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the schedule exclusion
        /// Checks if the tasks id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save()
        {
            log.Debug("starts-Save() method");
            ExecutionContext machineUserContext =  executionContext;
            ScheduleExclusionDataHandler scheduleExclusionDataHandler = new ScheduleExclusionDataHandler();
            if (scheduleExclusionDTO.ScheduleExclusionId < 0)
            {
                int ScheduleExclusionId = scheduleExclusionDataHandler.InsertScheduleExclusion(scheduleExclusionDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                scheduleExclusionDTO.ScheduleExclusionId = ScheduleExclusionId;
            }
            else
            {
                if (scheduleExclusionDTO.IsChanged == true)
                {
                    scheduleExclusionDataHandler.UpdateScheduleExclusion(scheduleExclusionDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    scheduleExclusionDTO.AcceptChanges();
                }
            }
            log.Debug("Ends-Save() method");
        }
        /// <summary>
        /// Returns the exclusion days
        /// </summary>
        /// <param name="exclusionId">Exclusion id is integer type parameter</param>
        /// <param name="dtdate">DateTime type parameter</param>
        /// <returns>bool type value</returns>
        public bool GetExclusionDays(int exclusionId, DateTime dtdate)
        {
            log.LogMethodEntry(exclusionId, dtdate);
            ScheduleExclusionDataHandler scheduleExclusionDataHandler = new ScheduleExclusionDataHandler();
            log.LogMethodExit();
            return scheduleExclusionDataHandler.GetExclusionDays(exclusionId, dtdate);
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="scheduleExclusionDTO">Schedule Exclusion DTO</param>
        /// <param name="executionContext">executionContext</param>
        public ScheduleExclusion(ScheduleExclusionDTO scheduleExclusionDTO,ExecutionContext executionContext)
        {
            log.LogMethodEntry(scheduleExclusionDTO, executionContext);
            this.scheduleExclusionDTO = scheduleExclusionDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
    }
    /// <summary>
    /// Manages the list of schedule exclusion
    /// </summary>
    public class ScheduleExclusionList
    {
        private static readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ScheduleExclusionDTO> scheduleExclusionDTOs;
        private ExecutionContext executionContext;

        // Default Constructor
        public ScheduleExclusionList()
        {
            log.Debug("Starts-ScheduleExclusionList() default constructor.");
            this.scheduleExclusionDTOs = null;
            this.executionContext = ExecutionContext.GetExecutionContext();
            log.Debug("Ends-ScheduleExclusionList() default constructor.");
        }

        /// <summary>
        /// create the scheduleExclusionDTOs list object
        /// </summary>
        public ScheduleExclusionList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// create the scheduleExclusionDTOs list object
        /// </summary>
        /// <param name="scheduleExclusionDTOs"/>
        /// <paramref name="executionContext"/>
        public ScheduleExclusionList(List<ScheduleExclusionDTO> scheduleExclusionDTOs, ExecutionContext executionContext)
        {
            log.LogMethodEntry(scheduleExclusionDTOs, executionContext);
            this.scheduleExclusionDTOs = scheduleExclusionDTOs;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the Schedule exclusion list
        /// </summary>
        public List<ScheduleExclusionDTO> GetAllScheduleExclusions(List<KeyValuePair<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            ScheduleExclusionDataHandler scheduleExclusionDataHandler = new ScheduleExclusionDataHandler();
            List<ScheduleExclusionDTO> scheduleExclusionDTOs = scheduleExclusionDataHandler.GetScheduleExclusionList(searchParameters);
            log.LogMethodExit(scheduleExclusionDTOs);
            return scheduleExclusionDTOs;
        }
        /// <summary>
        /// Save and Updated the Schedule Exclusions details
        /// </summary>   
        public void SaveUpdateScheduleExclusionsList()
        {
            try
            {                              
                log.Debug("Beging-SaveUpdateScheduleExclusionsList()");
                if (scheduleExclusionDTOs != null)
                {
                    foreach (ScheduleExclusionDTO scheduleExclusionDTO in scheduleExclusionDTOs)
                    {
                        ScheduleExclusion scheduleExclusion = new ScheduleExclusion(scheduleExclusionDTO, executionContext);
                        scheduleExclusion.Save();
                    }
                }
                log.Debug("Ends-SaveUpdateScheduleExclusionsList()");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
        }
    }
}
