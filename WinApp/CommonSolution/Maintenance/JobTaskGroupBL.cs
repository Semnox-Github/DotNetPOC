/********************************************************************************************
 * Project Name - JOb Task Group
 * Description  - Bussiness logic of Job task group
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        11-Jan-2016   Raghuveera     Created 
 *2.70        08-Mar-2019   Guru S A       Renamed MaintenanceTaskGroup as JobTaskGroupBL
 *2.70        23-Apr-2019   Mehraj         Added SaveJobTaskGroup() method
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    ///Bussiness logic of job task group. It creates and modifies the job task group details
    /// </summary>
    public class JobTaskGroupBL
    {
        private JobTaskGroupDTO jobTaskGroupDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public JobTaskGroupBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            jobTaskGroupDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Constructor with the Id parameter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="jobTaskGroupId"></param>
        public JobTaskGroupBL(ExecutionContext executionContext, int jobTaskGroupId)
        {
            log.LogMethodEntry(executionContext, jobTaskGroupId);
            this.executionContext = executionContext;
            JobTaskGroupDataHandler jobTaskGroupDataHandler = new JobTaskGroupDataHandler(null);
            this.jobTaskGroupDTO = jobTaskGroupDataHandler.GetJobTaskGroup(jobTaskGroupId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="jobTaskGroupDTO">Parameter of the type jobTaskGroupDTO</param>
        public JobTaskGroupBL(ExecutionContext executionContext, JobTaskGroupDTO jobTaskGroupDTO)
        {
            log.LogMethodEntry(executionContext, jobTaskGroupDTO);
            this.executionContext = executionContext;
            this.jobTaskGroupDTO = jobTaskGroupDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the JOb tasks group
        /// Checks if the tasks id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (jobTaskGroupDTO.IsChanged == false
                    && jobTaskGroupDTO.JobTaskGroupId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            Validate(sqlTransaction);
            JobTaskGroupDataHandler jobTaskGroupDataHandler = new JobTaskGroupDataHandler(sqlTransaction);
            if (jobTaskGroupDTO.JobTaskGroupId < 0)
            {
                int maintenanceTaskGroupId = jobTaskGroupDataHandler.InsertJobTaskGroup(jobTaskGroupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                jobTaskGroupDTO.JobTaskGroupId = maintenanceTaskGroupId;
            }
            else
            {
                if (jobTaskGroupDTO.IsChanged == true)
                {
                    jobTaskGroupDataHandler.UpdateJobTaskGroup(jobTaskGroupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    jobTaskGroupDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        private void Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            JobTaskGroupDataHandler jobTaskGroupDataHandler = new JobTaskGroupDataHandler(sqlTransaction);
            List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>> searchParams = new List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>>();
            searchParams.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<JobTaskGroupDTO> jobTaskGroupDTOList = jobTaskGroupDataHandler.GetJobTaskGroupList(searchParams);
            if (jobTaskGroupDTOList != null && jobTaskGroupDTOList.Any())
            {
                if (jobTaskGroupDTOList.Exists(x => x.TaskGroupName == jobTaskGroupDTO.TaskGroupName && jobTaskGroupDTO.JobTaskGroupId == -1))
                {
                    log.Debug("Duplicate entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, " task group"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
                if (jobTaskGroupDTOList.Exists(x => x.TaskGroupName == jobTaskGroupDTO.TaskGroupName && x.JobTaskGroupId != jobTaskGroupDTO.JobTaskGroupId))
                {
                    log.Debug("Duplicate entries detail");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, " task group"), MessageContainerList.GetMessage(executionContext, "Validation Error")));
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public JobTaskGroupDTO JobTaskGroupDTO
        {
            get { return jobTaskGroupDTO; }
        }
    }
    /// <summary>
    /// Manages the list of Job task group
    /// </summary>
    public class JobTaskGroupList
    {
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<JobTaskGroupDTO> jobTaskGroupList;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public JobTaskGroupList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="jobTaskGroupList"></param>
        public JobTaskGroupList(ExecutionContext executionContext, List<JobTaskGroupDTO> jobTaskGroupList)
        {
            log.LogMethodEntry(executionContext, jobTaskGroupList);
            this.executionContext = executionContext;
            this.jobTaskGroupList = jobTaskGroupList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the job task group list
        /// </summary>
        public List<JobTaskGroupDTO> GetAllJobTaskGroups(List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            JobTaskGroupDataHandler jobTaskGroupDataHandler = new JobTaskGroupDataHandler(null);
            List<JobTaskGroupDTO> jobTaskGroupDTOList = jobTaskGroupDataHandler.GetJobTaskGroupList(searchParameters);
            log.LogMethodExit(jobTaskGroupDTOList);
            return jobTaskGroupDTOList;
        }

        /// <summary>
        /// Save or update Job task groups
        /// </summary>
        public void SaveJobTaskGroup()
        {
            try
            {
                log.LogMethodEntry();
                if (jobTaskGroupList != null)
                {
                    foreach (JobTaskGroupDTO jobTaskGroupDTO in jobTaskGroupList)
                    {
                        JobTaskGroupBL jobTaskGroupBL = new JobTaskGroupBL(executionContext, jobTaskGroupDTO);
                        jobTaskGroupBL.Save();
                    }
                }
                log.LogMethodExit();
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
                log.Error(ex.Message, ex);
                log.LogMethodExit(ex, ex.Message);
                throw;
            }
        }

    }
}
